using Moq;
using RENT.Domain.Dtos;

namespace Rent.Xunit.Auth
{
    public class AuthControllerTest
    {
        public void RegistrationTest()
        {
            var mockUserRegistration = new Mock<UserRegistrationDto>();
            var userRegistration = new UserRegistrationDto()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Name = "test_name",
                Surname = "test_surname",
                Email = "test@test.com",
                Occupation = "test_ocupation",
                Password = "test_password",
                PhoneNumber = "+4712345678",
                Roles = "Test_Admin"
            };
        }
    }
}