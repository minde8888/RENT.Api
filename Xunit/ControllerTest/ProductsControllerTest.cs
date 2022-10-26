using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RENT.Api.Controllers;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace Rent.Xunit.ControllerTest
{
    public class ProductsControllerTest
    {
        private readonly Mock<IProductsRepository> _mockProductRepository;
        private readonly Mock<IImagesService> _mockImageRepository;
        private readonly Mock<IProductsService> _mockProductService;
        private readonly Mock<IWebHostEnvironment> _mockWebHostRepository;
        private readonly Mock<IMapper> _mockMapperRepository;
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
            _mockImageRepository = new Mock<IImagesService>();
            _mockProductService = new Mock<IProductsService>();
            _mockWebHostRepository = new Mock<IWebHostEnvironment>();
            _mockMapperRepository = new Mock<IMapper>();

            _controller = new ProductsController(_mockImageRepository.Object,
                _mockWebHostRepository.Object,
                _mockProductService.Object,
                _mockProductRepository.Object,
                _mockMapperRepository.Object)
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
            // _mockProductRepository.Verify(x => x.AddProductsAsync(new ProducRequestDto()), Times.AtLeastOnce);

            var formFile = new Mock<IFormFile>();
            var imageName = _mockImageRepository.Setup(_ => _.SaveImage(formFile.Object, "1", "2")).Returns("");
            Assert.NotNull(imageName);
            //Assert.Equal("", imageName.);
        }

        [Fact]
        public void GetReturnsProductWithSameId()
        {
            var productList = GetProductsData();
            var productListDto = GetProductsDtoData();
            _mockProductRepository.Setup(x => x.GetProductIdAsync(productList[0].ProductsId)).ReturnsAsync(productList);
            _mockProductService.Setup(i => i.GetProductImage(productList, "test")).Returns(productListDto);
            //request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api/v1/?PageNumber=1&PageSize=5"));

            //var a = _mockProductService.Object;
            var response = _controller.GetAsync(productList[0].ProductsId.ToString());
            //Assert.NotNull(response);
            //Assert.NotNull(response.Content);
            //Assert.Equal(product[0], response.Result.Value.Count);
            //var productResult = Assert.IsType<List<Products>>(response);
            //Assert.Equal(1, productResult.Count);

            //// Act
            //IHttpActionResult actionResult = controller.Get(42);
            //var contentResult = actionResult as OkNegotiatedContentResult<Product>;

            //// Assert
            //Assert.IsNotNull(contentResult);
            //Assert.IsNotNull(contentResult.Content);
            //Assert.AreEqual(42, contentResult.Content.Id);
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
