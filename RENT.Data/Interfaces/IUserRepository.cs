using Microsoft.IdentityModel.Tokens;
using RENT.Api.Configuration;
using RENT.Domain.Dtos;
using RENT.Domain.Entities.Auth;
using System.Security.Claims;

namespace RENT.Data.Interfaces
{
    public interface IUserRepository
    {
        public Task AddUserAsync(UserRegistrationDto user);
        public Task<List<Temp>> GetUserInfo(ApplicationUser user, AuthResult token, string ImageSrc);
        public Task<bool> NewPassword(ResetPasswordRequest model);

        public Task<bool> SendEmailPasswordReset(ForgotPassword model, string origin, string token);

        public RefreshToken GetrefreshToken(SecurityToken token, string rand, ApplicationUser user);

        public Task<AuthResult> GenerateJwtTokenAsync(ApplicationUser user);

        public Task<AuthResult> VerifyToken(TokenRequests tokenRequest, ClaimsPrincipal principal, SecurityToken validatedToken);

        public string StringRandom();
    }
}


