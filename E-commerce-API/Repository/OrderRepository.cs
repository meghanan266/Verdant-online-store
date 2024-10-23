using E_commerce_API.DataModel;
using E_commerce_API.Domain.Models;
using E_commerce_API.Repository.Interface;
using Microsoft.IdentityModel.Tokens;
using Razorpay.Api;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace E_commerce_API.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ECommerceDbContext eCommerceDbContext;

        public OrderRepository(ECommerceDbContext eCommerceDbContext)
        {
            this.eCommerceDbContext = eCommerceDbContext;
        }

        public string CreateOrder(double totalPrice)
        {
            RazorpayClient client = new RazorpayClient("rzp_live_s7ESSdEI7eiPrY", "zYPh7lU0cu7b5w2m3Z5EsmVV");

            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", totalPrice * 100); // amount in the smallest currency unit
            options.Add("currency", "INR");
            Razorpay.Api.Order order = client.Order.Create(options);

            return Newtonsoft.Json.JsonConvert.SerializeObject(order.Attributes);
        }

        public void StoreTempOrderId(string tempOrderId, int userId)
        {
            eCommerceDbContext.Orders.Add(new Domain.Models.Order
            {
                Temp_Order_Id = tempOrderId,
                User_Id = userId
            });

            eCommerceDbContext.SaveChanges();
        }

        public bool MakePayment(PaymentResponseDto paymentResponse)
        {
            var order = eCommerceDbContext.Orders.First(u => u.Temp_Order_Id == paymentResponse.TempOrderId);
            order.Razorpay_Order_Id = paymentResponse.Razorpay_order_id;
            order.Razorpay_Payment_Id = paymentResponse.Razorpay_payment_id;
            order.Razorpay_Signature = paymentResponse.Razorpay_signature;

            eCommerceDbContext.SaveChanges();
            Dictionary<string, string> attributes = new Dictionary<string, string>();

            attributes.Add("razorpay_payment_id", paymentResponse.Razorpay_payment_id);
            attributes.Add("razorpay_order_id", paymentResponse.Razorpay_order_id);
            attributes.Add("razorpay_signature", paymentResponse.Razorpay_signature);
            try
            {
                Utils.verifyPaymentSignature(attributes);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        ////private static string GenerateSignature(string data, string signatureKey)
        ////{
        ////    var keyByte = Encoding.UTF8.GetBytes(signatureKey);
        ////    using (var hmacsha256 = new HMACSHA256(keyByte))
        ////    {
        ////        hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        ////        return hmacsha256.Hash.Aggregate("", (current, t) => current + t.ToString("X2")).ToLower();
        ////    }
        ////}

        ////private bool VerifyPaymentSignature(string data, string signatureKey)
        ////{
        ////    //var generated_signature = GenerateSignature(data, signatureKey);

        ////    if (generated_signature == signatureKey)
        ////    {
        ////        return true;
        ////    }
        ////    return false;
        ////}
        ///

        public void StoreSuccessfulOrder(SuccessfulOrderDTO successfulOrder, int userId)
        {
            successfulOrder.ProductList.ForEach(c =>
            {
                var order = new SuccessfulOrder
                {
                    User_Id = userId,
                    Product_Id = c.Product.ProductId,
                    Razor_Pay_Order_Id = successfulOrder.RazorPayOrderId,
                    Order_Date = DateTime.Now,
                    Delivery_Address = successfulOrder.DeliveryAddress,
                    Quantity = c.Quantity,
                    Product_Price = Convert.ToInt32(Math.Round(c.Product.Price * c.Quantity * (1m - (c.Product.Discount ?? 0) / 100m), MidpointRounding.AwayFromZero)),
                };

                eCommerceDbContext.SuccessfulOrders.Add(order);
                eCommerceDbContext.SaveChanges();
                order.Custom_Order_Id = this.GenerateCustomOrderID(order.Successful_Order_Id);
            });


            eCommerceDbContext.SaveChanges();
        }

        private string GenerateCustomOrderID(int orderID)
        {
            var yearMonth = DateTime.Now.ToString("yyyyMM");

            if (orderID > 99999)
            {
                return $"{yearMonth}{orderID.ToString()}";
            }

            return $"{yearMonth}{orderID.ToString().PadLeft(5, '0')}";
        }


        public List<SuccessfulOrderDTO> GetMyOrders(int userId)
        {
            var list = (from o in eCommerceDbContext.SuccessfulOrders
                        join j in eCommerceDbContext.Products on o.Product_Id equals j.Product_Id
                        where o.User_Id == userId
                        select new SuccessfulOrderDTO
                        {
                            SuccessfulOrderId = o.Successful_Order_Id,
                            DeliveryAddress = o.Delivery_Address,
                            OrderDate = o.Order_Date,
                            Product = new CartItem
                            {
                                Product = new ProductDto
                                {
                                    ProductName = j.Product_Name,
                                    PictureUrl = j.Picture_Url != null ? j.Picture_Url.Split(';', StringSplitOptions.None).ToList() : new List<string>(),
                                },
                                Quantity = o.Quantity
                            },
                            DeliveryTrackingId = o.Delivery_Tracking_Id,
                            ProductPrice = o.Product_Price,
                            CustomOrderId = o.Custom_Order_Id,
                        }).OrderByDescending(o => o.OrderDate).ToList();
            ////foreach (var item in list)
            ////{
            ////    if (!string.IsNullOrEmpty(item.DeliveryTrackingId))
            ////    {
            ////        item.DeliveryTracking = this.GetDeliveryTrackingInfo(item.DeliveryTrackingId).Result;
            ////    }
            ////}

            return list;
        }

        private async Task<DeliveryTracking> GetDeliveryTrackingInfo(string shiprocketId)
        {
            string token = await GetShippingAuthToken();

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, $"https://apiv2.shiprocket.in/v1/external/courier/track/awb/{shiprocketId}");
                request.Headers.Add("Authorization", $"Bearer {token}");
                var content = new StringContent(string.Empty);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var deliveryTrackingDetails = JsonSerializer.Deserialize<DeliveryTrackingDetails>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                return new DeliveryTracking
                {
                    TrackUrl = deliveryTrackingDetails.TrackingData.TrackUrl,
                    CurrentStatus = deliveryTrackingDetails.TrackingData.ShipmentTrack[0].CurrentStatus,
                };
            }
        }


        private async Task<string> GetShippingAuthToken()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, "https://apiv2.shiprocket.in/v1/external/auth/login");
            var content = new StringContent("{\n    \"email\": \"shreyapnaidu88@gmail.com\",\n    \"password\": \"Shreyanaidu15#\"\n}", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(jsonString).RootElement.GetProperty("token").GetString();
        }

        public List<SuccessfulOrderDTO> GetAllOrders(string filterValue)
        {
            var list = (from o in eCommerceDbContext.SuccessfulOrders
                        join j in eCommerceDbContext.Products on o.Product_Id equals j.Product_Id
                        select new SuccessfulOrderDTO
                        {
                            SuccessfulOrderId = o.Successful_Order_Id,
                            DeliveryAddress = o.Delivery_Address,
                            OrderDate = o.Order_Date,
                            Product = new CartItem
                            {
                                Product = new ProductDto
                                {
                                    ProductName = j.Product_Name,
                                },
                                Quantity = o.Quantity
                            },
                            DeliveryTrackingId = o.Delivery_Tracking_Id,
                            DeliveryStatus = o.Delivery_Status,
                            RazorPayOrderId = o.Razor_Pay_Order_Id,
                            UserId = o.User_Id,
                            ProductPrice = o.Product_Price,
                            CustomOrderId = o.Custom_Order_Id,
                        }).OrderByDescending(o => o.OrderDate).ToList();

            if (!string.IsNullOrEmpty(filterValue))
            {
                list = list.Where(l =>
                    (filterValue == "yes" && l.DeliveryStatus) ||
                    (filterValue == "no" && !l.DeliveryStatus)
                ).ToList();
            }
            return list;
        }

        public List<SuccessfulOrderDTO> SaveDashboardOrder(List<SuccessfulOrderDTO> successfulOrder)
        {
            var orderIds = successfulOrder.Select(o => o.SuccessfulOrderId);
            var orders = eCommerceDbContext.SuccessfulOrders.Where(s => orderIds.Contains(s.Successful_Order_Id)).ToList();
            foreach (var order in orders)
            {
                var successOrder = successfulOrder.FirstOrDefault(o => o.SuccessfulOrderId == order.Successful_Order_Id);
                order.Delivery_Tracking_Id = successOrder.DeliveryTrackingId.Trim();
                order.Delivery_Status = successOrder.DeliveryStatus;
                order.Modified_Date = DateTime.Now;
            }

            eCommerceDbContext.SaveChanges();
            return this.GetAllOrders(null);
        }
    }
}
