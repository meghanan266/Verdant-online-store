using E_commerce_API.DataModel;
using E_commerce_API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System.Security.Claims;

namespace E_commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUserRepository userRepository;
        private int userId;

        public OrderController(IOrderRepository orderRepository, IUserRepository userRepository)
        {
            this.orderRepository = orderRepository;
            this.userRepository = userRepository;
        }

        [HttpGet("create-order")]
        public string CreateOrder(double totalPrice)
        {
            return this.orderRepository.CreateOrder(totalPrice);
        }

        [HttpPost, Authorize]
        [Route("store-order-id")]
        public void StoreTempOrderId([FromBody] string tempOrderId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            this.userId = this.userRepository.GetUserIdFromToken(email);
            this.orderRepository.StoreTempOrderId(tempOrderId, userId);
        }

        [HttpPost]
        [Route("make-payment")]
        public bool MakePayment(PaymentResponseDto paymentResponse)
        {
            var status = this.orderRepository.MakePayment(paymentResponse);
            return status;
        }
        
        [HttpPost, Authorize]
        [Route("store-success-order")]
        public void StoreSuccessfulOrder(SuccessfulOrderDTO successfulOrder)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            this.userId = this.userRepository.GetUserIdFromToken(email);
            this.orderRepository.StoreSuccessfulOrder(successfulOrder, userId);
        }
        
        [HttpGet, Authorize]
        [Route("get-my-orders")]
        public List<SuccessfulOrderDTO> GetMyOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            this.userId = this.userRepository.GetUserIdFromToken(email);
            return this.orderRepository.GetMyOrders(userId);
        }
        
        [HttpGet, Authorize]
        [Route("get-all-orders")]
        public List<SuccessfulOrderDTO> GetAllOrders(string filterValue)
        {
            return this.orderRepository.GetAllOrders(filterValue);
        }

        [HttpPost, Authorize]
        [Route("save-dashboard-order")]
        public IActionResult SaveDashboardOrder(List<SuccessfulOrderDTO> successfulOrder)
        {
            return Ok(this.orderRepository.SaveDashboardOrder(successfulOrder));
        }
    }
}
