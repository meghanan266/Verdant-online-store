using E_commerce_API.DataModel;
using E_commerce_API.Domain.Models;

namespace E_commerce_API.Repository.Interface
{
    public interface IUserRepository
    {
        public TokenDto Authenticate(UserDto user);
        public string AddUser(UserDto user);
        public string UpdateUser(UserDto user);
        public TokenDto RefreshToken(TokenDto tokenDto);
        public List<AddressDto> GetAddress(int userId);
        public List<AddressDto> AddAddress(AddressDto address, int userId);
        public Location GetLocation(string pincode);
        public List<AddressDto> DeleteAddress(int addressId);
        public int GetUserIdFromToken(string email);
        public int SendEmail(string email);
        public int ResetPassword(ResetPasswordDto resetPasswordDto);
        public List<UserDto> GetAllUsers(int userId);
    }
}
