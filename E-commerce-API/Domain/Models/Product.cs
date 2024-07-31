#nullable disable

using System.ComponentModel.DataAnnotations;

namespace E_commerce_API.Domain.Models
{
    public class Product
    {
        [Key]
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_Description { get; set; }
        public int Price { get; set; }
        public string Picture_Url { get; set; }
        public string Product_Quantity { get; set; }
        public int? Discount { get; set; }
    }
}
