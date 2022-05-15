using Microsoft.IdentityModel.Tokens;
using RENT.Api.Configuration;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos.Requests;
using RENT.Domain.Entities.Auth;
using RENT.Services.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Xunit.Auth
{
    public class MockUserRepository : IUserRepository
    {
        public Task AddUserAsync(UserRegistrationDto user)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResult> GenerateJwtTokenAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public RefreshToken GetrefreshToken(SecurityToken token, string rand, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserInformationDto>> GetUserInfo(ApplicationUser user, AuthResult token, string ImageSrc)
        {
            throw new NotImplementedException();
        }

        public Task<bool> NewPassword(ResetPasswordRequest model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendEmailPasswordReset(ForgotPassword model, string origin, string token)
        {
            throw new NotImplementedException();
        }

        public string StringRandom()
        {
            throw new NotImplementedException();
        }

        public Task<AuthResult> VerifyToken(TokenRequests tokenRequest, ClaimsPrincipal principal, SecurityToken validatedToken)
        {
            throw new NotImplementedException();
        }
    }
}
