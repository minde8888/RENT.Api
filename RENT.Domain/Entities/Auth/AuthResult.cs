using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace RENT.Api.Configuration
{
    public class AuthResult: IdentityResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool AuthSuccess { get; set; }
        public List<string> AuthErrors { get; set; }
    }
}
