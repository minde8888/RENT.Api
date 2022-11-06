using Microsoft.AspNetCore.Identity;
using RENT.Data.Interfaces.IServices;
using RENT.Domain.Entities.Auth;

namespace RENT.Services.Services
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailPasswordService _mailService;
        public ResetPasswordService(UserManager<ApplicationUser> userManager,
            IEmailPasswordService mailService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        }

        public async Task<bool> NewPassword(ResetPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return false;
            }
            else if (user.ResetToken != model.Token
                && user.ResetTokenExpires < DateTime.UtcNow)
            {
                return false;
            }
            else
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (resetPassResult.Succeeded)
                {
                    user.PasswordReset = DateTime.UtcNow;
                    user.ResetToken = null;
                    user.ResetTokenExpires = null;
                    await _userManager.UpdateAsync(user);

                    return true;
                }
            }
            throw new Exception();
        }

        public async Task<bool> SendEmailPasswordReset(ForgotPassword model, string origin)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (token != null)
            {
                user.ResetToken = token;
                user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);
                await _userManager.UpdateAsync(user);
            }
            else
            {
                throw new ArgumentException("Token is null");
            }
            var link = $"{origin}/api/Auth/NewPassword?token={token}&email={user.Email}";
            bool sendEmail = _mailService.SendEmailPasswordReset(model, link);
            return sendEmail;
        }
    }
}
