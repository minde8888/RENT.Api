using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RENT.Api.Controllers;
using RENT.Data.Filter;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Dtos.ResponseDto;
using RENT.Domain.Entities;
using System.Runtime.Versioning;

namespace Rent.Xunit.ControllerTest
{
    public class ProductsControllerTest
    {
        private readonly Mock<IProductsRepository> _mockProductRepository;
        private readonly Mock<IProductsService> _mockProductService;
        private readonly ProductsController _controller;
        private readonly string Url = "http://localhost:44346/api/v1/Products";

        public ProductsControllerTest()
        {
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Scheme).Returns("http");
            request.Setup(x => x.Host).Returns(HostString.FromUriComponent("localhost:44346"));
            request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api/v1/Products"));
            request.Setup(x => x.Headers["token"]).Returns("fake_token_here");
            request.Setup(x => x.Path).Returns("/api/v1/Products");
            var httpContext = Mock.Of<HttpContext>(_ =>
                _.Request == request.Object
            );

            _mockProductRepository = new Mock<IProductsRepository>();
            _mockProductService = new Mock<IProductsService>();

            _controller = new ProductsController(
                _mockProductService.Object,
                _mockProductRepository.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        [SupportedOSPlatform("windows")]
        public void AddProduct()
        {
            //Arrange
            var product = GetProductRequestDto();
            _mockProductService.Setup(x => x.AddProductWithImage(product));
            //Act
            var response = _controller.AddNewProductAsync(product);
            //result
            Assert.Equal(typeof(Task<ActionResult>), response.GetType());
        }

        [Fact]
        public void GetAllProductsByFromQuery()
        {
            //Arrange
            var product = GetProductResponseDto();
            var filter = GetPaginationFilter();
            _mockProductService.Setup(x => x.GetAllProductsAsync(filter, Url, "/api/v1/Products")).ReturnsAsync(product);
            //Act
            var response = _controller.GetAllAsync(filter);
            var result = response.Result.Result as OkObjectResult;
            //result
            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(typeof(Task<ActionResult<ProductResponseDto>>), response.GetType());
            Assert.Equal(product, result.Value);
        }

        [Fact]
        public void GetReturnsProductWithSameId()
        {
            //Arrange
            var productList = GetProductsData();
            var productListDto = GetProductsDtoData();
            _mockProductService.Setup(x => x.GetProductById(productListDto[0].ProductsId, Url)).ReturnsAsync(productListDto[0]);
            //Act
            var response = _controller.GetAsync(productList[0].ProductsId.ToString());
            var result = response.Result.Result as OkObjectResult;
            // Assert
            Assert.NotNull(response);
            Assert.NotNull(result);
            Assert.Equal(typeof(Task<ActionResult<List<ProductsDto>>>), response.GetType());
            Assert.Equal(productListDto[0], result.Value);
        }

        [Fact]
        [SupportedOSPlatform("windows")]
        public void UpdateProductValues()
        {
            //Arrange
            _mockProductService.Setup(x => x.UpdateItemAsync(GetProductRequestDto()));
            //Act
            var response = _controller.UpdateAsync(GetProductRequestDto());
            // Assert
            Assert.Equal(typeof(OkResult), response.GetType());
        }

        [Fact]
        [SupportedOSPlatform("windows")]
        public void DeleteProduct()
        {
            //Arrange
            var id = Guid.NewGuid();
            _mockProductRepository.Setup(x => x.RemoveProductsAsync(id.ToString()));
            //Act
            var response = _controller.DeleteProductAsync(id.ToString());
            // Assert
            Assert.Equal(typeof(Task<ActionResult>), response.GetType());
        }

        private ProductsRequestDto GetProductRequestDto()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization()
            {
                ConfigureMembers = true
            });

            return fixture.Create<ProductsRequestDto>();
        }

        private List<Products> GetProductsData()
        {
            List<Products> productsData = new()
            {
                new Products() { ProductsId = new Guid("E4DE1CC1-5271-4B2F-9783-A96E9F904DE9") },
                new Products() { ProductsId = new Guid("BAA849FE-5270-4E34-AFCA-0C94D7522166") },
                new Products() { ProductsId = new Guid("197C5532-C5CA-4A31-AF68-C1A54A3D06C4") }
            };
            return productsData;
        }

        private List<ProductsDto> GetProductsDtoData()
        {
            List<ProductsDto> productsData = new()
            {
                new ProductsDto() { ProductsId = new Guid("E4DE1CC1-5271-4B2F-9783-A96E9F904DE9") },
                new ProductsDto() { ProductsId = new Guid("BAA849FE-5270-4E34-AFCA-0C94D7522166") },
                new ProductsDto() { ProductsId = new Guid("197C5532-C5CA-4A31-AF68-C1A54A3D06C4") }
            };
            return productsData;
        }

        private PaginationFilter GetPaginationFilter()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization()
            {
                ConfigureMembers = true
            });
            return fixture.Create<PaginationFilter>();
        }
        private ProductResponseDto GetProductResponseDto()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization()
            {
                ConfigureMembers = true
            });
            return fixture.Create<ProductResponseDto>();
        }
    }
}