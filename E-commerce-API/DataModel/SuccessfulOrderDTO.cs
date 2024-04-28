using E_commerce_API.Domain.Models;

namespace E_commerce_API.DataModel
{
    public class SuccessfulOrderDTO
    {
        public int SuccessfulOrderId { get; set; }
        public List<ProductDto> ProductList { get; set; }
        public string RazorPayOrderId { get; set; }
        public string DeliveryAddress { get; set; }
        public Nullable<DateTime> OrderDate { get; set; }
    }
}
