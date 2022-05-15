using System.ComponentModel.DataAnnotations;

namespace RENT.Domain.Entities.Auth
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}