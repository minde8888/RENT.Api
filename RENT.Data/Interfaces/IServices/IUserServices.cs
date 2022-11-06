using RENT.Api.Configuration;
using RENT.Api.Configuration.Requests;
using RENT.Domain.Dtos;

namespace RENT.Data.Interfaces.IServices
{
    public interface IUserServices
    {
        Task<AuthResult> CreateNewUserAsync(UserRegistrationDto user);
        Task<object> UserInfo(UserLoginRequest user, string imageSrc);
    }
}
