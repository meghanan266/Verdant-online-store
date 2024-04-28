using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_API.Domain.Models
{
    public class Cart
    {
        [Key]
        public int Cart_Id { get; set; }
        public int Product_Id { get; set; }
        public int Product_Quantity { get; set; }
        public int User_Id { get; set; }
    }
}
