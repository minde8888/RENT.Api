using RENT.Api.Configuration;
using RENT.Domain.Dtos;
using RENT.Domain.Entities.Auth;

namespace RENT.Data.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        public Task AddNewUserAsync(UserRegistrationDto user);
        public Task<UserInformationDto> GetUserInfo(ApplicationUser user, AuthResult token, string ImageSrc);
    }
}


