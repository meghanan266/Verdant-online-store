namespace E_commerce_API.DataModel
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int Price { get; set; }
        public string[] PictureUrl { get; set; }
        public string ProductQuantity { get; set; }
    }
}
