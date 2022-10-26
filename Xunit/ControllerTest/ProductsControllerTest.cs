using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RENT.Api.Controllers;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;
using System.Net;

namespace Rent.Xunit.ControllerTest
{
    public class ProductsControllerTest
    {
        private readonly Mock<IProductsRepository> _mockProductRepository;
        private readonly Mock<IProductsService> _mockProductService;
        private readonly ProductsController _controller;

        public ProductsControllerTest()
        {
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Scheme).Returns("http");
            request.Setup(x => x.Host).Returns(HostString.FromUriComponent("https://localhost:44346/"));
            request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api/v1/"));
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
                    HttpContext = new DefaultHttpContext()
                }
            };
        }
        [Fact]
        public void AddProduct()
        {
            //Arrange
            var product = new ProducRequestDto();
           _mockProductService.Setup(x => x.AddProductWithImage(product));
            //Act
            var response = _controller.AddNewProductAsync(product);
            //result.  
            Assert.Equal(typeof(Task<IActionResult>), response.GetType());
        }

        [Fact]
        public void GetAllProductsByFromQuery()
        {
            //request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api/v1/?PageNumber=1&PageSize=5"));
        }
        [Fact]
        public void GetReturnsProductWithSameId()
        {
            //Arrange
            var productList = GetProductsData();
            var productListDto = GetProductsDtoData();
            _mockProductService.Setup(x => x.GetProductById(productList[0].ProductsId, "imageSrc")).ReturnsAsync(productListDto);  
            //Act
            var response = _controller.GetAsync(productList[0].ProductsId.ToString());
            // Assert
            Assert.NotNull(response);
            //Assert.NotNull(response.Content);
            //Assert.Equal(mockResult., response);
            //var productResult = Assert.IsType<List<Products>>(response);
            //Assert.Equal(1, productResult.Count);
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
        private List<ProductDto> GetProductsDtoData()
        {
            List<ProductDto> productsData = new()
            {
                new ProductDto() { ProductsId = new Guid("E4DE1CC1-5271-4B2F-9783-A96E9F904DE9") },
                new ProductDto() { ProductsId = new Guid("BAA849FE-5270-4E34-AFCA-0C94D7522166") },
                new ProductDto() { ProductsId = new Guid("197C5532-C5CA-4A31-AF68-C1A54A3D06C4") }
            };
            return productsData;
        }
    }
}
