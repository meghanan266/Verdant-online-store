namespace E_commerce_API.DataModel
{
    public class CartDto
    {
        public List<CartItem> CartItems { get; set; }
        public int TotalPrice { get; set; }
    }
}
