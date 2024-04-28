using E_commerce_API.DataModel;
using E_commerce_API.Domain.Models;
using E_commerce_API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;  
using System.Security.Claims;

namespace E_commerce_API.Controllers
{
    [Route("api/[controller] ")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository cartRepository;
        private readonly IUserRepository userRepository;

        public CartController(ICartRepository cartRepository, IUserRepository userRepository)
        {
            this.cartRepository = cartRepository;
            this.userRepository = userRepository;
        }

        [HttpGet, Authorize]
        [Route("getAllCartItems")]
        public CartDto GetAllCartItems()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var userId = this.userRepository.GetUserIdFromToken(email);
            return this.cartRepository.GetAllCartItems(userId);
        }

        [HttpPost, Authorize]
        [Route("addToCart")]
        public IActionResult AddToCart(CartItem cart)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var userId = this.userRepository.GetUserIdFromToken(email);
            this.cartRepository.AddToCart(cart, userId);
            return Ok();
        }

        [HttpPost, Authorize]
        [Route("removeFromCart")]
        public IActionResult RemoveFromCart(CartItem cart)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var userId = this.userRepository.GetUserIdFromToken(email);
            this.cartRepository.RemoveFromCart(cart, userId);
            return Ok();
        }

        [HttpDelete, Authorize]
        [Route("empty-cart")]
        public void EmptyCart()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var userId = this.userRepository.GetUserIdFromToken(email);
            cartRepository.EmptyCart(userId);
        }
    }
}
