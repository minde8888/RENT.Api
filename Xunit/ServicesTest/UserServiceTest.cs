using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Identity;
using Moq;
using RENT.Data.Interfaces.IRepositories;
using RENT.Data.Interfaces.IServices;
using RENT.Domain.Dtos;
using RENT.Domain.Entities.Auth;
using RENT.Services.Services;

namespace Rent.Xunit.ServicesTest
{
    public class UserServiceTest
    {
        private readonly Mock<IUserStore<ApplicationUser>> _mockUserManager;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ITokenService> _tokenService;
        private readonly UserManager<ApplicationUser> _mockUser;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _mockUserManager = new Mock<IUserStore<ApplicationUser>>(); ;
            _userRepository = new Mock<IUserRepository>();
            _tokenService = new Mock<ITokenService>();
            _mockUser = new UserManager<ApplicationUser>(_mockUserManager.Object, null, null, null, null, null, null, null, null);

            _userService = new UserService(
             _mockUser,
            _userRepository.Object,
            _tokenService.Object);
        }
        [Fact]
        public void CreateNewUserAsync()
        {
            //Arrange
            var user = GetUser();
            var userDto = GetUserDto();

            var dbName = nameof(UserServiceTest.CreateNewUserAsync);
            var context = DbContextInMemory.GetApplicationDbContext(dbName, user);
            context.Users.Any(u =>
                u.PhoneNumber ==
                user.PhoneNumber ||
                u.Email ==
                user.Email);
            _mockUser.CreateAsync(user, userDto.Password);
            _userRepository.Setup(x => x.AddNewUserAsync(userDto));
            //Act
            var response = _userService.CreateNewUserAsync(userDto);

        }

        private ApplicationUser GetUser()
        {
            return new ApplicationUser()
            {
                Roles = "Roles",
                Email = "Email",
                UserName = "StringRandom()",
                PhoneNumber = "PhoneNumber"
            };
        }

        private UserRegistrationDto GetUserDto()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization()
            {
                ConfigureMembers = true
            });
            return fixture.Create<UserRegistrationDto>();
        }
    }
}
