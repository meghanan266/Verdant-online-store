using E_commerce_API.Domain.Models;

namespace E_commerce_API.DataModel
{
    public class SuccessfulOrderDTO
    {
        public int SuccessfulOrderId { get; set; }
        public List<CartItem> ProductList { get; set; }
        public CartItem Product { get; set; }
        public string RazorPayOrderId { get; set; }
        public string DeliveryAddress { get; set; }
        public Nullable<DateTime> OrderDate { get; set; }
        public bool DeliveryStatus { get; set; }
        public string DeliveryTrackingId { get; set; }
        public int UserId {  get; set; }
        public DeliveryTracking DeliveryTracking { get; set; }
        public int ProductPrice { get; set; }
        public string CustomOrderId { get; set; }
    }
}
