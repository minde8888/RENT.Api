using AutoFixture.AutoMoq;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RENT.Api.Controllers;
using RENT.Data.Filter;
using RENT.Data.Interfaces;
using RENT.Data.Repository;
using RENT.Domain.Dtos;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Rent.Xunit.ControllerTest
{
    public class CategoryControllerTest
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly CategoryController _controller;

        public CategoryControllerTest()
        {
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Scheme).Returns("http");
            request.Setup(x => x.Host).Returns(HostString.FromUriComponent("https://localhost:44346/"));
            request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api/v1/"));
            var httpContext = Mock.Of<HttpContext>(_ =>
                _.Request == request.Object
            );

            _mockCategoryRepository = new Mock<ICategoryRepository>();

            _controller = new CategoryController(_mockCategoryRepository.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        [SupportedOSPlatform("windows")]
        public void AddNewCategory()
        {
            //Arrange
            var category = GetCategoriesDto();
            _mockCategoryRepository.Setup(x => x.AddCategotyAsync(category)).ReturnsAsync(category);
            //Act
            var response = _controller.AddNewCategory(category);
            var result = response.Result as OkObjectResult;
            //result
            Assert.NotNull(result);
            Assert.Equal(category, result.Value);

        }
        private CategoriesDto GetCategoriesDto()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization()
            {
                ConfigureMembers = true
            });
           
           return fixture.Create<CategoriesDto>();
        }
    }
}
