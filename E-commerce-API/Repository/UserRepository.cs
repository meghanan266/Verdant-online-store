using AutoMapper;
using E_commerce_API.DataModel;
using E_commerce_API.Domain.Models;
using E_commerce_API.Helper;
using E_commerce_API.Repository.Interface;
using E_commerce_API.Services;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace E_commerce_API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ECommerceDbContext eCommerceDbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IEmailService emailService;
        public UserRepository(ECommerceDbContext eCommerceDbContext, IMapper mapper, IEmailService emailService, IConfiguration configuration)
        {
            this.eCommerceDbContext = eCommerceDbContext;
            this.mapper = mapper;
            this.configuration = configuration;
            this.emailService = emailService;
        }

        public TokenDto Authenticate(UserDto user)
        {
            var exisitingUser = this.eCommerceDbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (exisitingUser == null)
                return null;
            if (!PasswordHasher.VerifyPassword(user.Password, exisitingUser.Password))
            {
                return null;
            }

            exisitingUser.Token = CreateJwt(exisitingUser);
            var newRefreshToken = CreateRefreshToken();
            exisitingUser.Refresh_Token = newRefreshToken;
            exisitingUser.Refresh_Token_Expiry_Time = DateTime.Now.AddDays(5);
            eCommerceDbContext.SaveChanges();
            return new TokenDto
            {
                AccessToken = exisitingUser.Token,
                RefreshToken = newRefreshToken
            };
        }

        public string AddUser(UserDto user)
        {
            //check if email already exists
            if (this.eCommerceDbContext.Users.Any(u => u.Email == user.Email))
                return null;
            user.Password = PasswordHasher.HashPassword(user.Password);
            var newUser = new User
            {
                User_Name = user.UserName,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                Role = "User"
            };
            this.eCommerceDbContext.Users.Add(newUser);
            this.eCommerceDbContext.SaveChanges();
            return CreateJwt(newUser);
        }

        public string UpdateUser(UserDto user)
        {
            var exisitingUser = this.eCommerceDbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if (exisitingUser == null)
                return null;

            exisitingUser.Email = user.Email;
            exisitingUser.User_Name = user.UserName;
            exisitingUser.Phone = user.Phone;
            exisitingUser.Password = user.Password != null ? PasswordHasher.HashPassword(user.Password) : exisitingUser.Password;
            this.eCommerceDbContext.Update(exisitingUser);
            this.eCommerceDbContext.SaveChanges();

            return CreateJwt(exisitingUser);
        }

        private string CreateJwt(User user)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("secretkey12345678");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("phone", user.Phone),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.User_Name),
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddMinutes(5),
                SigningCredentials = credentials,
                NotBefore = DateTime.Now,
            };

            var token = jwtHandler.CreateToken(tokenDescriptor);
            return jwtHandler.WriteToken(token);
        }

        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var user = eCommerceDbContext.Users.Any(user => user.Refresh_Token == refreshToken);

            if (user)
                return CreateRefreshToken();

            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretkey12345678")),
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
            };
            var jwtHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = jwtHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("This is invalid token");
            }

            return principal;
        }

        public TokenDto RefreshToken(TokenDto tokenDto)
        {
            if (tokenDto.AccessToken != null && tokenDto.RefreshToken != null)
            {
                string accessToken = tokenDto.AccessToken;
                string refreshToken = tokenDto.RefreshToken;
                var principal = GetPrincipalFromExpiredToken(accessToken);
                var email = principal.FindFirstValue(ClaimTypes.Email);
                var user = eCommerceDbContext.Users.FirstOrDefault(u => u.Email == email);
                if (user is null || user.Refresh_Token != refreshToken || user.Refresh_Token_Expiry_Time <= DateTime.Now)
                {
                    return null;
                }

                var newAccessToken = CreateJwt(user);
                var newRefreshToken = CreateRefreshToken();
                user.Refresh_Token = newRefreshToken;
                user.Token = newAccessToken;
                eCommerceDbContext.SaveChanges();
                return new TokenDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                };
            }
            return null;
        }

        public List<AddressDto> GetAddress(int userId)
        {
            var addressList = eCommerceDbContext.Addresses.Where(a => a.User_Id == userId).Select(a => new AddressDto
            {
                Address = a.Address_Desc,
                Pincode = a.Pincode,
                City = a.City,
                State = a.State,
                Locality = a.Locality,
                AddressId = a.Address_Id
            }).ToList();

            return addressList;
        }

        public List<AddressDto> AddAddress(AddressDto address, int userId)
        {
            if (address.AddressId == null)
            {
                eCommerceDbContext.Addresses.Add(new Address
                {
                    Address_Desc = address.Address,
                    Pincode = address.Pincode,
                    State = address.State,
                    City = address.City,
                    Locality = address.Locality,
                    User_Id = userId,
                });
            }
            else
            {
                var addressToBeEdited = eCommerceDbContext.Addresses.First(a => a.Address_Id == address.AddressId);
                addressToBeEdited.Address_Desc = address.Address;
                addressToBeEdited.Pincode = address.Pincode;
                addressToBeEdited.State = address.State;
                addressToBeEdited.City = address.City;
                addressToBeEdited.Locality = address.Locality;
            }

            eCommerceDbContext.SaveChanges();

            var addressList = eCommerceDbContext.Addresses.Where(a => a.User_Id == userId).Select(a => new AddressDto
            {
                Address = a.Address_Desc,
                Pincode = a.Pincode,
                City = a.City,
                State = a.State,
                Locality = a.Locality,
                AddressId = a.Address_Id
            }).ToList();

            return addressList;
        }

        public Location GetLocation(string pincode)
        {
            return eCommerceDbContext.Locations.FirstOrDefault(a => a.Pincode.ToString() == pincode);
        }

        public List<AddressDto> DeleteAddress(int addressId)
        {
            var addressToBeDeleted = eCommerceDbContext.Addresses.First(a => a.Address_Id == addressId);
            var userId = addressToBeDeleted.User_Id;
            eCommerceDbContext.Addresses.Remove(addressToBeDeleted);

            eCommerceDbContext.SaveChanges();

            var addressList = eCommerceDbContext.Addresses.Where(a => a.User_Id == userId).Select(a => new AddressDto
            {
                Address = a.Address_Desc,
                Pincode = a.Pincode,
                City = a.City,
                State = a.State,
                Locality = a.Locality,
                AddressId = a.Address_Id
            }).ToList();

            return addressList;
        }

        public int GetUserIdFromToken(string email)
        {
            return this.eCommerceDbContext.Users.First(u => u.Email == email).User_Id;
        }

        public int SendEmail(string email)
        {
            var user = eCommerceDbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return 404;
            }
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.Reset_Password_Token = emailToken;
            user.Reset_Password_Expiry = DateTime.Now.AddMinutes(15);
            string from = configuration["EmailSettings:From"];
            var emailModel = new EmailDTO(email, "Reset Password", EmailBody.EmailStringBody(email, emailToken));
            emailService.SendEmail(emailModel);
            eCommerceDbContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            eCommerceDbContext.SaveChanges();
            return 200;
        }

        public int ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");
            var user = eCommerceDbContext.Users.FirstOrDefault(u => resetPasswordDto.Email == u.Email);
            if (user == null)
            {
                return 404;
            }
            var token = user.Reset_Password_Token;
            if (token != resetPasswordDto.EmailToken || (user.Reset_Password_Expiry != null && user.Reset_Password_Expiry < DateTime.Now))
            {
                return 400;
            }
            user.Password = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);
            eCommerceDbContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            eCommerceDbContext.SaveChanges();
            return 200;
        }

        public List<UserDto> GetAllUsers(int userId)
        {
            if (eCommerceDbContext.Users.FirstOrDefault(u => u.User_Id == userId).Role == "Admin")
            {
                return eCommerceDbContext.Users.Select(u => new UserDto
                {
                    UserId = u.User_Id,
                    UserName = u.User_Name,
                    Email = u.Email,
                    Phone = u.Phone,
                }).ToList();
            }

            return null;
        }
    }
}
