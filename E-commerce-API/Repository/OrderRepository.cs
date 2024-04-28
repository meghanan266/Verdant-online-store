using E_commerce_API.DataModel;
using E_commerce_API.Domain.Models;
using E_commerce_API.Repository.Interface;
using Microsoft.IdentityModel.Tokens;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace E_commerce_API.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ECommerceDbContext eCommerceDbContext;

        public OrderRepository(ECommerceDbContext eCommerceDbContext)
        {
            this.eCommerceDbContext = eCommerceDbContext;
        }

        public string CreateOrder(int totalPrice)
        {
            RazorpayClient client = new RazorpayClient("rzp_test_anzIjjz2Vd1MvW", "e5uoVcUJk0L3GDDdx1svsG9H");

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
                eCommerceDbContext.SuccessfulOrders.Add(new SuccessfulOrder
                {
                    User_Id = userId,
                    Product_Id = c.ProductId,
                    Razor_Pay_Order_Id = successfulOrder.RazorPayOrderId,
                    Order_Date = DateTime.Now,
                    Delivery_Address = successfulOrder.DeliveryAddress
                });
            });

            eCommerceDbContext.SaveChanges();
        }

        public List<SuccessfulOrderDTO> GetMyOrders(int userId)
        {
            var list = (from o in eCommerceDbContext.SuccessfulOrders
                        where o.User_Id == userId
                        select new SuccessfulOrderDTO
                        {
                            SuccessfulOrderId = o.Successful_Order_Id,
                            DeliveryAddress = o.Delivery_Address,
                            OrderDate = o.Order_Date,
                            ProductList = (from c in eCommerceDbContext.Products
                                        where c.Product_Id == o.Product_Id
                                        select new ProductDto
                                        {
                                            PictureUrl = c.Picture_Url.Split(';', StringSplitOptions.None),
                                            ProductName = c.Product_Name,
                                        }).ToList(),
                        }).OrderByDescending(o => o.OrderDate).ToList();

            return list;
        }
    }
}
