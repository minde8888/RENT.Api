using System.ComponentModel.DataAnnotations;


namespace RENT.Domain.Entities.Auth
{
    public class TokenRequests
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
