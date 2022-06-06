using Microsoft.Extensions.Options;
using RENT.Data.Interfaces;
using RENT.Domain.Entities.Auth;
using RENT.Domain.Entities.Settings;
using System.Net.Mail;

namespace RENT.Services.Services
{
    public class EmailPasswordService : IEmailPasswordService
    {
        private readonly MailSettings _mailSettings;

        public EmailPasswordService(MailSettings mailSettings)
        {
            _mailSettings = mailSettings ?? throw new ArgumentNullException(nameof(mailSettings));
        }

        public EmailPasswordService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public bool SendEmailPasswordReset(ForgotPassword model, string link)
        {
            MailMessage mailMessage = new();
            mailMessage.From = new MailAddress(_mailSettings.Mail);
            mailMessage.To.Add(new MailAddress(model.Email));
            mailMessage.CC.Add(new MailAddress("cc@email.com", "CC Name"));
            mailMessage.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
            mailMessage.Subject = "Password Reset";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = $"<H1>Try to click the link below<H1/> <br/> <div>{link}<div/>" +
                $"<div>click link to reset </div>";

            SmtpClient client = new()
            {
                EnableSsl = _mailSettings.UseSSL,
                Credentials = new System.Net.NetworkCredential(_mailSettings.Mail, _mailSettings.Password),
                Host = _mailSettings.Host,
                Port = _mailSettings.Port,
            };
           
            var a = mailMessage;
            client.Send(mailMessage);
            return true;
        }
    }
}
