using Microsoft.IdentityModel.Tokens;
using RENT.Api.Configuration;
using RENT.Domain.Dtos;
using RENT.Domain.Entities.Auth;
using System.Security.Claims;

namespace RENT.Data.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        public Task AddNewUserAsync(UserRegistrationDto user);
        public Task<UserInformationDto> GetUserInfo(ApplicationUser user, AuthResult token, string ImageSrc);
    }
}


