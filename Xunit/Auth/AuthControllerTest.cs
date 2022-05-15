using Microsoft.IdentityModel.Tokens;
using RENT.Api.Controllers;

namespace Rent.Xunit.Auth
{
    public class AuthControllerTest
    {
        public void LoginTest()
        {
            var tokenValidationParams = new TokenValidationParameters();
            var repo = new MockUserRepository();
            //var constroller = new AuthController(tokenValidationParams, repo);
        }
    }
}