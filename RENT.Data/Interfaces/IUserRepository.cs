using RENT.Domain.Dtos.Requests;

namespace RENT.Data.Interfaces
{
    public interface IUserRepository
    {
        public Task AddUserAsync(UserRegistrationDto user);
    }
}


