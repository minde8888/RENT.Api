﻿using Microsoft.Extensions.Options;
using System.Net.Mail;
using RENT.Data.Interfaces;
using RENT.Domain.Entities.Auth;
using RENT.Domain.Entities.Settings;

namespace RENT.Services.Services
{
    public class EmailPassword : IEmailPasswordService
    {
        private readonly MailSettings _mailSettings;

        public EmailPassword(MailSettings mailSettings)
        {
            _mailSettings = mailSettings ?? throw new ArgumentNullException(nameof(mailSettings));
        }

        public EmailPassword(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public bool SendEmailPasswordReset(ForgotPassword model, string link)
        {
            MailMessage mailMessage = new()
            {
                From = new MailAddress(_mailSettings.Mail)
            };
            mailMessage.To.Add(new MailAddress(model.Email));

            mailMessage.Subject = "Password Reset";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = $"<H1>Try to click the link below<H1/> <br/> <div>{link}<div/>" +
                $"<div>click link to reset </div>";

            SmtpClient client = new()
            {
                EnableSsl = true,
                Credentials = new System.Net.NetworkCredential(_mailSettings.Mail, _mailSettings.Password),
                Host = _mailSettings.Host,
                Port = _mailSettings.Port
            };

            client.Send(mailMessage);
            return true;
        }
    }
}
