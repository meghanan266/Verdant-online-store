using System.ComponentModel.DataAnnotations;

namespace E_commerce_API.Domain.Models
{
    public class Review
    {
        [Key]
        public int Review_Id { get; set; }
        public int User_Id { get; set; }
        public int Product_Id { get; set; }
        public string Review_Description { get; set; }
        public bool Top_Review { get; set; }
        public DateTime Review_Date { get; set; }
        public int Review_Range { get; set; }
    }
}
