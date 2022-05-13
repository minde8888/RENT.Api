using Microsoft.Extensions.Options;
using System.Net.Mail;
using WTP.Data.Interfaces;
using WTP.Domain.Entities.Auth;
using WTP.Domain.Entities.Settings;

namespace WTP.Services.Services
{
    public class EmailPassword : IEmailPassword
    {
        private readonly MailSettings _mailSettings;

        public EmailPassword(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public bool SendEmailPasswordReset(ForgotPassword model, string link)
        {
            MailMessage mailMessage = new();
            mailMessage.From = new MailAddress(_mailSettings.Mail);
            mailMessage.To.Add(new MailAddress(model.email));

            mailMessage.Subject = "Password Reset";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = $"<H1>Try to click the link below<H1/> <br/> <div>{link}<div/>" +
                $"<div>click link to reset </div>";

            SmtpClient client = new()
            {
                EnableSsl = true
            };
            client.Credentials = new System.Net.NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
            client.Host = _mailSettings.Host;
            client.Port = _mailSettings.Port;

            client.Send(mailMessage);
            return true;
        }
    }
}
