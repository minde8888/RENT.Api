using RENT.Api.Configuration;
using RENT.Api.Configuration.Requests;
using RENT.Domain.Dtos;

namespace RENT.Data.Interfaces.IServices
{
    public interface IUserService
    {
        Task<AuthResult> CreateNewUserAsync(UserRegistrationDto user);
        Task<AuthResult> UserInfo(UserLoginRequest user, string imageSrc);
    }
}
