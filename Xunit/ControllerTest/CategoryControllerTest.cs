using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RENT.Api.Controllers;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos;
using System.Runtime.Versioning;

namespace Rent.Xunit.ControllerTest
{
    public class CategoryControllerTest
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly CategoryController _controller;
        public CategoryControllerTest()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _controller = new CategoryController(_mockCategoryRepository.Object);
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

        [Fact]
        public void GetAllCategories()
        {
            //Arrange
            var category = GetCategoriesDto();
            var listCategories = new List<CategoriesDto>
            {
                category,
                category,
                category
            };
            _mockCategoryRepository.Setup(x => x.GetAllCategoriesAsync()).ReturnsAsync(listCategories);
            //Act
            var response = _controller.GetAll();
            var result = response.Result.Result as OkObjectResult;
            //result
            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(typeof(Task<ActionResult<CategoriesDto>>), response.GetType());
            Assert.Equal(listCategories, result.Value);
        }

        [Fact]
        public void GetReturnCategoryWithSameId()
        {
            //Arrange
            var category = GetCategoriesDto();
            _mockCategoryRepository.Setup(x => x.GetCategoriesIdAsync(category.CategoriesId)).ReturnsAsync(category);
            //Act
            var response = _controller.Get(category.CategoriesId.ToString());
            var result = response.Result.Result as OkObjectResult;
            //result
            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(typeof(Task<ActionResult<CategoriesDto>>), response.GetType());
            Assert.Equal(category, result.Value);
        }

        [Fact]
        [SupportedOSPlatform("windows")]
        public void UpdatweCategoryValues()
        {
            //Arrange
            var category = GetCategoriesDto();
            _mockCategoryRepository.Setup(x => x.UpdateCategory(category));
            //Act
            var response = _controller.Update(category);
            // Assert
            Assert.Equal(typeof(OkResult), response.GetType());
        }

        [Fact]
        [SupportedOSPlatform("windows")]
        public void DeleteCategory()
        {
            //Arrange
            var id = Guid.NewGuid();
            _mockCategoryRepository.Setup(x => x.RemoveCategoryAsync(id.ToString()));
            //Act
            var response = _controller.Delete(id.ToString());
            // Assert
            Assert.Equal(typeof(OkResult), response.GetType());
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