using RENT.Domain.Entities;

namespace RENT.Data.Repositorys
{
    public interface IMailReposidory
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendWelcomeEmailAsync(WelcomeRequest request);
    }
}
