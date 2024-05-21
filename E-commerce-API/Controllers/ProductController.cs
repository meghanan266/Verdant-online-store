using E_commerce_API.DataModel;
using E_commerce_API.Domain.Models;
using E_commerce_API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IUserRepository userRepository;

        public ProductController(IProductRepository productRepository, IUserRepository userRepository)
        {
            this.productRepository = productRepository;
            this.userRepository = userRepository;
        }

        [HttpGet]
        [Route("products-list")]
        public List<ProductDto> GetAllProducts()
        {
            return this.productRepository.GetAllProducts();
        }

        [HttpGet]
        [Route("product-item/{productId}")]
        public ProductDto GetProduct(int productId)
        {
            return this.productRepository.GetProduct(productId);
        }

        [HttpGet]
        [Route("get-review/{productId}")]
        public List<ReviewDTO> GetReview(int productId)
        {
            return this.productRepository.GetReview(productId);
        }

        [HttpPost, Authorize]
        [Route("add-review")]
        public int AddReview(ReviewDTO review)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var userId = this.userRepository.GetUserIdFromToken(email);
            int statusCode = productRepository.AddReview(review, userId);
            if (statusCode == 200)
            {
                return 200;
            }
            return 404;
        }

        [HttpPost, Authorize]
        [Route("save-product")]
        public List<ProductDto> SaveProduct(List<ProductDto> products)
        {
            return productRepository.SaveProduct(products);
        }
    }
}
