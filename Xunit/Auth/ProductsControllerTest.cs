using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using RENT.Api.Controllers;
using RENT.Data.Interfaces;
using RENT.Domain.Entities;

namespace Rent.Xunit.Auth
{
    public class ProductsControllerTest
    {
        private readonly Mock<IProductsRepository> _mockProductRepository;
        private readonly Mock<IImagesService> _mockImageRepository;
        private readonly Mock<IProductsService> _mockProductService;
        private readonly Mock<IWebHostEnvironment> _mockWebHostRepository;
        private readonly ProductsController _controller;

        public ProductsControllerTest()
        {
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Scheme).Returns("http");
            request.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
            request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api/v1/"));
            var httpContext = Mock.Of<HttpContext>(_ =>
                _.Request == request.Object
            );
            _mockProductRepository = new Mock<IProductsRepository>();
            _mockImageRepository = new Mock<IImagesService>();
            _mockProductService = new Mock<IProductsService>();
            _mockWebHostRepository = new Mock<IWebHostEnvironment>();
            _controller = new ProductsController(_mockImageRepository.Object,
                _mockWebHostRepository.Object,
                _mockProductService.Object,
                _mockProductRepository.Object)
            {
                ControllerContext = new ProductsController()
                {
                    HttpContext = httpContext,
                }
            };
        }

        [Fact]
        public void GetReturnsProductWithSameId()
        {
            Guid g = Guid.NewGuid();
            List<Products> product = new();
            product.Add(new Products() { ProductsId = g });
            _mockProductRepository.Setup(x => x.GetProductIdAsync(g)).ReturnsAsync(product);


            //request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/v1/customersupport-manager/logs/account/D4C88E3C-2848-400F-AB66-0DC3FAFFD24A?PageNumber=1&PageSize=5"));

            //var a = mockProductRepository.Object;


            //.ReturnsAsync(new Products() { ProductsId = g});


            //var controller = new ProductsController(mockImageRepository.Object, mockWebHostRepository.Object, mockProductService.Object, mockProductRepository.Object);
            var response = _controller.GetAsync(g.ToString()).Result as ObjectResult; ;
            Assert.NotNull(response);
            //    Assert.Equal(product.Count(), response.Result.Value.Count);
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
    }
}
