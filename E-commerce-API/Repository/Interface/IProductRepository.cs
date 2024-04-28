using E_commerce_API.DataModel;
using E_commerce_API.Domain.Models;

namespace E_commerce_API.Repository.Interface
{
    public interface IProductRepository
    {
        public List<ProductDto> GetAllProducts();
        public ProductDto GetProduct(int productId);
        public int AddReview(ReviewDTO review, int userId);
        public List<ReviewDTO> GetReview(int productId);
    }
}