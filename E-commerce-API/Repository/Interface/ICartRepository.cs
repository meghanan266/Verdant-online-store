using E_commerce_API.DataModel;

namespace E_commerce_API.Repository.Interface
{
    public interface ICartRepository
    {
        public void AddToCart(CartItem cartItem, int userId);
        public CartDto GetAllCartItems(int userId);
        void RemoveFromCart(CartItem cart, int userId);
        public void EmptyCart(int userId);
    }
}
