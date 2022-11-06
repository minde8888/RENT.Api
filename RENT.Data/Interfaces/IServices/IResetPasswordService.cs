using RENT.Domain.Entities.Auth;

namespace RENT.Data.Interfaces.IServices
{
    public interface IResetPasswordService
    {
        public Task<bool> NewPassword(ResetPasswordRequest model);
        public Task<bool> SendEmailPasswordReset(ForgotPassword model, string origin);
    }
}
