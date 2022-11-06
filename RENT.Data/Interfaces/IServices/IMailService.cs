using RENT.Domain.Entities;

namespace RENT.Data.Interfaces.IServices
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendWelcomeEmailAsync(WelcomeRequest request);
    }
}
