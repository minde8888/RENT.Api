using System.ComponentModel.DataAnnotations;

namespace WTP.Domain.Entities.Auth
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
