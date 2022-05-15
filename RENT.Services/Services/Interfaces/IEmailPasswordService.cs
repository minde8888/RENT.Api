﻿using RENT.Domain.Entities.Auth;

namespace RENT.Data.Interfaces
{
    public interface IEmailPasswordService
    {
        public bool SendEmailPasswordReset(ForgotPassword model, string link);
    }
}
