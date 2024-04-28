using System.ComponentModel.DataAnnotations;

namespace E_commerce_API.Domain.Models
{
    public class Order
    {
        [Key]
        public int Order_Id { get; set; }
        public string Temp_Order_Id { get; set; }
        public int User_Id { get; set; }
        public string Razorpay_Payment_Id { get; set; }
        public string Razorpay_Order_Id { get; set; }
        public string Razorpay_Signature { get; set; }
    }
}
