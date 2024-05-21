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
        private enum RowStatus
        {
            NEW = 1,
            EDITED = 2,
            DELETED = 3,
        };
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
                PictureUrl = p.Picture_Url != null ? p.Picture_Url.Split(';', StringSplitOptions.None).ToList() : new List<string>(),
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

        public List<ProductDto> SaveProduct(List<ProductDto> products)
        {
            var newProducts = products.Where(p => p.RowStatus == Convert.ToInt16(RowStatus.NEW)).Select(p => new Product
            {
                Product_Name = p.ProductName,
                Product_Description = p.ProductDescription,
                Price = p.Price,
                Product_Quantity = p.ProductQuantity
            });
            eCommerceDbContext.Products.AddRange(newProducts);

            var deletedIds = products.Where(p => p.RowStatus == Convert.ToInt16(RowStatus.DELETED)).Select(p => p.ProductId).ToList();
            var deletedProducts = eCommerceDbContext.Products.Where(pr => deletedIds.Contains(pr.Product_Id));
            eCommerceDbContext.Products.RemoveRange(deletedProducts);

            products.Where(p => p.RowStatus != Convert.ToInt16(RowStatus.NEW) || p.RowStatus == Convert.ToInt16(RowStatus.DELETED)).ToList().ForEach(p =>
                {
                    var product = eCommerceDbContext.Products.First(pr => pr.Product_Id == p.ProductId);
                    product.Price = p.Price;
                    product.Product_Quantity = p.ProductQuantity;
                    product.Product_Name = p.ProductName;
                    product.Product_Description = p.ProductDescription;
                });

            eCommerceDbContext.SaveChanges();

            return this.GetAllProducts();
        }
    }
}
