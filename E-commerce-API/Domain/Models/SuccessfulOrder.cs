using E_commerce_API.DataModel;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_API.Domain.Models
{
    public class SuccessfulOrder
    {
        [Key]
        public int Successful_Order_Id { get; set; }
        public string Custom_Order_Id { get; set; }
        public int User_Id { get; set; }
        public int Product_Id { get; set; }
        public string Razor_Pay_Order_Id { get; set; }
        public DateTime Order_Date { get; set; }
        public string Delivery_Address { get; set; }
        public bool Delivery_Status { get; set; }
        public string Delivery_Tracking_Id { get; set; }
        public int Quantity { get; set; }
        public DateTime? Modified_Date { get; set; }
        public int Product_Price {  get; set; }
    }
}
