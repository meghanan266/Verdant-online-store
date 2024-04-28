using AutoMapper;
using E_commerce_API.DataModel;
using E_commerce_API.Domain.Models;
using E_commerce_API.Repository.Interface;

namespace E_commerce_API.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ECommerceDbContext eCommerceDbContext;
        private readonly IMapper mapper;

        public CartRepository(ECommerceDbContext eCommerceDbContext, IMapper mapper)
        {
            this.eCommerceDbContext = eCommerceDbContext;
            this.mapper = mapper;
        }
        public void AddToCart(CartItem cartItem, int userId)
        {
            var item = eCommerceDbContext.Carts.FirstOrDefault(c => c.Product_Id == cartItem.Product.ProductId && c.User_Id == userId);
            if (item != null)
            {
                item.Product_Quantity = cartItem.Quantity;
                this.eCommerceDbContext.Carts.Update(item);
            }
            else
            {
                item = new Cart
                {
                    Product_Id = cartItem.Product.ProductId,
                    Product_Quantity = cartItem.Quantity,
                    User_Id = userId
                };
                this.eCommerceDbContext.Carts.Add(item);
            }

            this.eCommerceDbContext.SaveChanges();
        }

        public CartDto GetAllCartItems(int userId)
        {
            var cartDto = new CartDto
            {
                CartItems = (from cart in this.eCommerceDbContext.Carts
                             where cart.User_Id == userId
                             select new CartItem
                             {
                                 Quantity = cart.Product_Quantity,
                                 Product = mapper.Map<ProductDto>(this.eCommerceDbContext.Products.FirstOrDefault(p => p.Product_Id == cart.Product_Id)),
                             }).ToList(),
                TotalPrice = 0
            };
            cartDto.CartItems.ForEach(c =>
            {
                cartDto.TotalPrice += c.Quantity * c.Product.Price;
            });
            return cartDto;
        }

        public void RemoveFromCart(CartItem cart, int userId)
        {

            var item = eCommerceDbContext.Carts.FirstOrDefault(c => c.Product_Id == cart.Product.ProductId && c.User_Id == userId);
            if (item != null)
            {
                this.eCommerceDbContext.Carts.Remove(item);
            }

            this.eCommerceDbContext.SaveChanges();
        }

        public void EmptyCart(int userId)
        {
            var cartList = eCommerceDbContext.Carts.Where(c => c.User_Id == userId).ToList();
            eCommerceDbContext.Carts.RemoveRange(cartList);
            eCommerceDbContext.SaveChanges();
        }
    }
}
