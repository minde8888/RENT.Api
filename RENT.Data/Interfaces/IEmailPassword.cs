using WTP.Domain.Entities.Auth;

namespace WTP.Data.Interfaces
{
    public interface IEmailPassword
    {
        public bool SendEmailPasswordReset(ForgotPassword model, string link);
    }
}
