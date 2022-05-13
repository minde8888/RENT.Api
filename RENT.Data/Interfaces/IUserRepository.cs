using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using WTP.Domain.Dtos.Requests;
using WTP.Domain.Entities.Auth;

namespace WTP.Data.Interfaces
{
    public interface IUserRepository
    {
        public Task AddUserAsync(UserRegistrationDto user);
    }
}


