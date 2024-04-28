using AutoMapper;
using E_commerce_API.DataModel;
using E_commerce_API.Domain.Models;
using E_commerce_API.Repository.Interface;

namespace E_commerce_API.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ECommerceDbContext eCommerceDbContext;
        private readonly IMapper mapper;

        public ProductRepository(ECommerceDbContext eCommerceDbContext, IMapper mapper)
        {
            this.eCommerceDbContext = eCommerceDbContext;
            this.mapper = mapper;
        }

        public List<ProductDto> GetAllProducts()
        {
            List<ProductDto> productList = this.eCommerceDbContext.Products.Select(p => new ProductDto
            {
                ProductId = p.Product_Id,
                ProductName = p.Product_Name,
                Price = p.Price,
                PictureUrl = p.Picture_Url.Split(';', StringSplitOptions.None),
                ProductDescription = p.Product_Description,
                ProductQuantity = p.Product_Quantity,
            }).ToList();

            return productList;
        }

        public ProductDto GetProduct(int productId)
        {
            var product = this.eCommerceDbContext.Products.FirstOrDefault(p => p.Product_Id == productId);
            return mapper.Map<ProductDto>(product);
        }

        public int AddReview(ReviewDTO review, int userId)
        {
            var orderExists = eCommerceDbContext.SuccessfulOrders.Any(o => o.User_Id == userId && review.ProductId == o.Product_Id);
            if (orderExists)
            {
                eCommerceDbContext.Reviews.Add(new Review
                {
                    User_Id = userId,
                    Product_Id = review.ProductId,
                    Review_Description = review.ReviewDescription,
                    Review_Range = review.ReviewRange,
                    Review_Date = DateTime.Now,
                });

                eCommerceDbContext.SaveChanges();
                return 200;
            }
            else
            {
                return 400;
            }
        }

        public List<ReviewDTO> GetReview(int productId)
        {
            return (from r in eCommerceDbContext.Reviews
                    where r.Product_Id == productId
                    select new ReviewDTO
                    {
                        ProductId = r.Product_Id,
                        ReviewDescription = r.Review_Description,
                        ReviewDate = r.Review_Date,
                        UserName = eCommerceDbContext.Users.First(u => u.User_Id == r.User_Id).User_Name,
                        ReviewRange = r.Review_Range
                    }).ToList();
        }
    }
}
