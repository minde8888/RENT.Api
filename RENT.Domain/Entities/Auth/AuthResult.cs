using RENT.Domain.Dtos;

namespace RENT.Api.Configuration
{
    public class AuthResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public UserInformationDto UserInformationDto { get; set; }
    }
}
