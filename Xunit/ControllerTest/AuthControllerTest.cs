using AutoFixture.AutoMoq;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using RENT.Api.Controllers;
using RENT.Data.Filter;
using RENT.Data.Interfaces;
using RENT.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RENT.Domain.Dtos;
using RENT.Data.Repositorys;
using static RENT.Api.Configuration.Roles.Authorization;

namespace Rent.Xunit.ControllerTest
{
    public class AuthControllerTest
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<TokenValidationParameters> _mockTokenValidationParams;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly AuthController _controller;

        public AuthControllerTest()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>();
            _mockTokenValidationParams = new Mock<TokenValidationParameters>();
            _mockUserRepository = new Mock<IUserRepository>();

            _controller = new AuthController(
                _mockUserManager.Object,
                _mockTokenValidationParams.Object,
                _mockUserRepository.Object);
        }

        [Fact]
        public void SignupNewuser()
        {
            //Arrange
            var user = GetUser();
            _mockUserManager.Setup(x => x.Users.Any(u => u.Id == user.Id)).Returns(true);
            var newUser = GetAppUser();
           _mockUserRepository.Setup(x => x.StringRandom()).Returns("random_name");
           _mockUserManager.Setup(c => c.CreateAsync(newUser, user.Password))
                .ReturnsAsync(IdentityResult.Success).Verifiable();
            _mockUserManager.Setup(x => x.AddToRoleAsync(newUser, user.Roles));
            user.UserId = newUser.Id;
            _mockUserRepository.Setup(x => x.AddUserAsync(user));
            //Act
            var response = _controller.Register(user);

        }

        private UserRegistrationDto GetUser()
        {
            var user = new UserRegistrationDto()
            {
                Id = new Guid("197C5532-C5CA-4A31-AF68-C1A54A3D06C4"),
                UserId = new Guid("197C5532-C5CA-4A31-AF68-C1A54A3D06C5"),
                Name = "test_Name",
                Surname = "test_Surname",
                PhoneNumber = "123456789",
                Email = "test_email",
                Occupation = "test_occupation",
                Password = "test_password",
                Roles = "test_role"
            };
   
            return user;
        }
        private ApplicationUser GetAppUser()
        {
            var user = GetUser();
            var newUser = new ApplicationUser()
            {
                Roles = user.Roles,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return newUser;
        }
    }
}
