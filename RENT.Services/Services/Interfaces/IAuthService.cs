using Microsoft.IdentityModel.Tokens;
using RENT.Api.Configuration;
using RENT.Domain.Entities.Auth;
using RENT.Services.Services.Dtos;
using System.Security.Claims;

namespace RENT.Services.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<List<UserInformationDto>> GetUserInfo(ApplicationUser user, AuthResult token, string ImageSrc);
        public Task<bool> NewPassword(ResetPasswordRequest model);

        public Task<bool> SendEmailPasswordReset(ForgotPassword model, string origin, string token);

        public RefreshToken GetrefreshToken(SecurityToken token, string rand, ApplicationUser user);

        public Task<AuthResult> GenerateJwtTokenAsync(ApplicationUser user);

        public Task<AuthResult> VerifyToken(TokenRequests tokenRequest, ClaimsPrincipal principal, SecurityToken validatedToken);

        public string StringRandom();
    }
}
