using System.ComponentModel.DataAnnotations;


namespace WTP.Domain.Entities.Auth
{
    public class TokenRequests
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
