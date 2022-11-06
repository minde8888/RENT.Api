using Microsoft.IdentityModel.Tokens;
using RENT.Api.Configuration;
using RENT.Domain.Entities.Auth;
using System.Security.Claims;


namespace RENT.Data.Interfaces.IServices
{
    public interface ITokenService
    {
        Task<AuthResult> VerifyToken(TokenRequests tokenRequest, ClaimsPrincipal principal, SecurityToken validatedToken);
        Task<AuthResult> GenerateJwtTokenAsync(ApplicationUser user);
    }
}
