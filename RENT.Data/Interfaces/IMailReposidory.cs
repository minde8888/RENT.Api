using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.Domain.Entities;

namespace WTP.Data.Repositorys
{
    public interface IMailReposidory
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendWelcomeEmailAsync(WelcomeRequest request);
    }
}
