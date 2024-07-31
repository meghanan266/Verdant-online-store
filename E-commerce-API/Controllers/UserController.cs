using E_commerce_API.DataModel;
using E_commerce_API.Domain.Models;
using E_commerce_API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly ICartRepository cartRepository;
        private int userId;

        public UserController(IUserRepository userRepository, ICartRepository cartRepository)
        {
            this.userRepository = userRepository;
            this.cartRepository = cartRepository;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(UserDto user)
        {
            return Ok(this.userRepository.Authenticate(user));
        }

        [HttpPost("add-user")]
        public IActionResult AddUser([FromBody] UserDto user)
        {
            return Ok(new
            {
                Token = this.userRepository.AddUser(user)
            });
        }

        [HttpPost("update-user")]
        public IActionResult UpdateUser([FromBody] UserDto user)
        {
            return Ok(this.userRepository.UpdateUser(user));
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken(TokenDto tokenDto)
        {
            if (tokenDto == null)
                return BadRequest("Invalid");

            return Ok(this.userRepository.RefreshToken(tokenDto));
        }

        [HttpGet, Authorize]
        [Route("get-address")]
        public IActionResult GetAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            this.userId = this.userRepository.GetUserIdFromToken(email);
            return Ok(this.userRepository.GetAddress(userId));
        }

        [HttpPost, Authorize]
        [Route("add-address")]
        public IActionResult AddAddress(AddressDto address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            this.userId = this.userRepository.GetUserIdFromToken(email);
            return Ok(this.userRepository.AddAddress(address, userId));
        }

        [HttpGet]
        [Route("get-location/{pincode}")]
        public IActionResult GetLocation(string pincode)
        {
            return Ok(this.userRepository.GetLocation(pincode));
        }

        [HttpDelete]
        [Route("delete-address/{addressId}")]
        public IActionResult DeleteAddress(int addressId)
        {
            return Ok(this.userRepository.DeleteAddress(addressId));
        }

        [HttpPost("send-reset-email")]
        public IActionResult SendEmail([FromBody] string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var statusCode = userRepository.SendEmail(email);
                if (statusCode == 200)
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Email sent"
                    });
                }
            }

            return NotFound(new
            {
                StatusCode = 404,
                Message = "User doesn't exist"
            });
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var statusCode = userRepository.ResetPassword(resetPasswordDto);
            if (statusCode == 200)
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Password Reset Successful"
                });
            }
            else if (statusCode == 400)
            {
                return BadRequest(new
                {
                    statuscode = 400,
                    message = "Invalid reset link"
                });
            }
            return NotFound(new
            {
                StatusCode = 404,
                Message = "User doesn't exist"
            });

        }

        [HttpGet, Authorize]
        [Route("get-all-users")]
        public List<UserDto> GetAllUsers()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            this.userId = this.userRepository.GetUserIdFromToken(email);
            return this.userRepository.GetAllUsers(userId);
        }
    }
}
