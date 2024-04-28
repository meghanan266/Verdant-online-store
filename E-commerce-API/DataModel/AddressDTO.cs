namespace E_commerce_API.DataModel
{
    public class AddressDto
    {
        public int? AddressId { get; set; }
        public string State { get; set; }
        public string Locality { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public int UserId { get; set; }
    }
}
