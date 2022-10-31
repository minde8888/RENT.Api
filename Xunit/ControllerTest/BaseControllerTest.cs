using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using RENT.Api.Controllers;
using RENT.Data.Interfaces;
using RENT.Domain.Entities;
using System.Net.Http;

namespace Rent.Xunit.ControllerTest
{
    public class BaseControllerTest<T> : ControllerBase where T : BaseEntity
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        //private readonly Mock<IBaseRepository<T>> _mockBaseRepository;
        //private readonly Mock<IBaseSerrvice<T>> _mockBaseSerrvice;
        private readonly BaseController<Seller> _sellerController;
        private readonly BaseController<Customers> _customersController;
        private readonly Mock<IBaseRepository<Seller>> _mockSellerRepository;
        private readonly Mock<IBaseRepository<Customers>> _mockCustomersRepository;
        private readonly Mock<IBaseSerrvice<Seller>> _mockSellerSerrvice;
        private readonly Mock<IBaseSerrvice<Customers>> _mockCustomersSerrvice;

        public BaseControllerTest(IWebHostEnvironment hostEnvironment)
        {
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Scheme).Returns("http");
            request.Setup(x => x.Host).Returns(HostString.FromUriComponent("localhost:44346"));
            //request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api/v1/Products"));
            request.Setup(x => x.Headers["token"]).Returns("fake_token_here");
            //request.Setup(x => x.Path).Returns("/api/v1/Products");
            request.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()).Value).Returns("E4DE1CC1-5271-4B2F-9783-A96E9F904DE9");
            var httpContext = Mock.Of<HttpContext>(_ =>
                _.Request == request.Object               
            );
         //   Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);

            _hostEnvironment = hostEnvironment;
            _mockSellerRepository = new Mock<IBaseRepository<Seller>>();
            _mockSellerSerrvice = new Mock<IBaseSerrvice<Seller>>();
            _mockCustomersSerrvice = new Mock<IBaseSerrvice<Customers>>();
            _mockCustomersRepository = new Mock<IBaseRepository<Customers>>();
            //_mockBaseSerrvice = new Mock<IBaseSerrvice<T>>();
            //_mockBaseRepository = new Mock<IBaseRepository<T>>();
            _sellerController = new BaseController<Seller>(
                _mockSellerRepository.Object,
                _mockSellerSerrvice.Object,
                _hostEnvironment)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                    //HttpContext.User.FindFirstValue("id") = httpContext.User.FindFirstValue("id");
                    //.User.FindFirstValue("id");
                }
            };
            _customersController = new BaseController<Customers>(
                _mockCustomersRepository.Object,
                _mockCustomersSerrvice.Object,
                _hostEnvironment)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public void AddIem()
        {
            // Arrange
            var seller = GetSeller();
            var custumer = GetCustomers();
            _mockSellerRepository.Setup(x => x.AddItemAsync(seller, "E4DE1CC1-5271-4B2F-9783-A96E9F904DE9"));
            _mockCustomersRepository.Setup(x => x.AddItemAsync(custumer, "E4DE1CC1-5271-4B2F-9783-A96E9F904DE9"));
            //Act
            var responseSeller = _sellerController.AddNewItem(seller);
            var responseCustumer = _customersController.AddNewItem(custumer);
            var resultSeller = responseSeller.Result as OkObjectResult;
            var resultCustumer = responseCustumer.Result as OkObjectResult;
            //result
            Assert.NotNull(resultSeller);
            Assert.NotNull(resultCustumer);
            Assert.Equal(seller, resultSeller.Value);
            Assert.Equal(custumer, resultCustumer.Value);
        }

        private Seller GetSeller()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization()
            {
                ConfigureMembers = true
            });
            return fixture.Create<Seller>();
        }

        private Customers GetCustomers()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization()
            {
                ConfigureMembers = true
            });
            return fixture.Create<Customers>();
        }
    }
}