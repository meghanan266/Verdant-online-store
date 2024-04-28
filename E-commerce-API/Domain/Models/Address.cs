using System.ComponentModel.DataAnnotations;

namespace E_commerce_API.Domain.Models
{
    public class Address
    {
        [Key]
        public int Address_Id { get; set; }
        public string State { get; set; }
        public string Locality { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string Address_Desc { get; set; }
        public int User_Id { get; set; }
    }
}
