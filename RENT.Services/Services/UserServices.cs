using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RENT.Api.Configuration;
using RENT.Api.Configuration.Requests;
using RENT.Data.Helpers;
using RENT.Data.Interfaces.IRepositories;
using RENT.Data.Interfaces.IServices;
using RENT.Domain.Dtos;
using RENT.Domain.Entities.Auth;
using System;
using System.Runtime.CompilerServices;

namespace RENT.Services.Services
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserServices(UserManager<ApplicationUser> userManager,
            TokenValidationParameters tokenValidationParams,
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _tokenValidationParams = tokenValidationParams ?? throw new ArgumentNullException(nameof(tokenValidationParams));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<AuthResult> CreateNewUserAsync(UserRegistrationDto user)
        {
            var exist = _userManager.Users.Any(u =>
                u.PhoneNumber ==
                user.PhoneNumber ||
                u.Email ==
                user.Email);

            if (exist)
            {
                return new AuthResult()
                {
                    Errors = new List<string>()
                        {
                            "Email or phone number is already in use !!!"
                        },
                    Success = false
                };
            }

            var newUser = new ApplicationUser()
            {
                Roles = user.Roles,
                Email = user.Email,
                UserName = StringRandom(),
                PhoneNumber = user.PhoneNumber
            };

            var isCreated = await _userManager.CreateAsync(newUser, user.Password);
            if (isCreated.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, user.Roles);
                user.UserId = newUser.Id;
                await _userRepository.AddNewUserAsync(user);
            }
            else
            {
                return new AuthResult()
                {
                    Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                    Success = false
                };
            }
            return new AuthResult()
            {
                Success = isCreated.Succeeded               
            };
        }

        public async Task<object> UserInfo(UserLoginRequest user, string imageSrc)
        {
            ApplicationUser existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser == null)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() {
                                "The email address is incorrect. Please retry."
                            },
                    Success = false
                };
            }
            if (existingUser.IsDeleted)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() {
                                "User account was deleted "
                            },
                    Success = false
                };
            }

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

            if (!isCorrect)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() {
                                "The password is incorrect. Please try again."
                            },
                    Success = false
                };
            }
            var token = await _tokenService.GenerateJwtTokenAsync(existingUser);
            var result = await _userRepository.GetUserInfo(existingUser, token, imageSrc);
            return result;
        }

        public string StringRandom() 
        {
            string unique = RandomString.RandString(36);

            var existName = _userManager.FindByNameAsync(unique);
            if (existName.Result != null)
            {
                StringRandom();
            }
            else
            {
                return unique;
            }
            throw new Exception();
        }
    }
}