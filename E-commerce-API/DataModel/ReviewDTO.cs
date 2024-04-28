namespace E_commerce_API.DataModel
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public string ReviewDescription { get; set; }
        public bool TopReview { get; set; }
        public DateTime ReviewDate { get; set; }
        public string UserName { get; set; }
        public int ReviewRange { get; set; }
    }
}
