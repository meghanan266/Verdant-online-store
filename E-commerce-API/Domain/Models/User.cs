using System.ComponentModel.DataAnnotations;

namespace E_commerce_API.Domain.Models
{
    public class User
    {
        [Key]
        public int User_Id { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string Refresh_Token { get; set; }
        public DateTime? Refresh_Token_Expiry_Time { get; set; }
        public string Reset_Password_Token { get; set; }
        public Nullable<DateTime> Reset_Password_Expiry { get; set; }
    }
}
