using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos.RequestDto;
using System.Runtime.Versioning;

namespace RENT.Api.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IImagesService _imagesService;
        private readonly IMapper _mapper;
        private readonly IProductsRepository _productsRepository;
        private readonly IProductsService _productsService;

        public ProductsController(IMapper mapper,
            IImagesService imagesService,
            IWebHostEnvironment hostEnvironment,
            IProductsService productsService,
            IProductsRepository productsRepository)
        {
            _mapper = mapper;
            _imagesService = imagesService;
            _hostEnvironment = hostEnvironment;
            _productsRepository = productsRepository;
            _productsService = productsService;
        }

        [HttpPost]
        [SupportedOSPlatform("windows")]
        public async Task<IActionResult> AddNewProduct([FromForm] ProducRequesttDto product)
        {
            try
            {
                var imageName = "";
                if (product.Images != null)
                {
                    string[] height = product.ImageHeight.Split(',');
                    string[] width = product.ImageWidth.Split(',');
                    string path = _hostEnvironment.ContentRootPath;

                    for (int i = 0; i < height.Length; i++)
                    {
                        imageName += _imagesService.SaveImage(product.Images[i], height[i], width[i]);
                    }
                }
                product.ImageName = imageName;
                await _productsRepository.AddProductsAsync(product);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error Get data from the database -> AddNewProduct");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ProductDto>>> GetAll()
        {
            try
            {
                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                return Ok(await _productsRepository.GetProductsAsync(ImageSrc));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error Get data from the database" + ex);
            }
        }

        [HttpGet("id")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ProductDto>>> Get(String id)
        {
            try
            {
                if (String.IsNullOrEmpty(id))
                    return BadRequest();
                var userId = new Guid(id);

                var product = await _productsRepository.GetProductIdAsync(userId);
                if (product == null)
                    return NotFound();

                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);

                var productsToReturn = _productsService.GetProductImageAsync(product, ImageSrc);

                return Ok(productsToReturn);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Could not find web user account");
            }
        }

        [HttpPut("Update")]
        [SupportedOSPlatform("windows")]
        public ActionResult Update([FromForm] ProducRequesttDto product)
        {
            if (product.ProductsId == Guid.Empty)
                return BadRequest("This project can not by updated");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                string[] height = product.ImageHeight.Split(',');
                string[] width = product.ImageWidth.Split(',');
                string[] imageName = product.ImageSrc.Split(',');
                int count = 0;

                for (int i = 0; i < imageName.Length; i++)
                {
                    if (imageName[i] == "/")
                    {
                        imageName[i] = _imagesService.SaveImage(product.Images[count], height[count], width[count]);
                        count++;
                    }
                }
                product.ImageName = imageName.ToString();
                _productsRepository.UpdateProductAsync(product);
                return Ok();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error update DB");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ex);
            }
        }

        [HttpPost("Delete")]
        [Authorize(Roles = "Seller, Admin")]
        public async Task<ActionResult> DeleteProduct([FromBody] List<object> ids)
        {
            foreach (var p in ids)
            {
                var id = new Guid(p.ToString());
                if (id == Guid.Empty)
                    return BadRequest();

                await _productsRepository.RemoveProductsAsync(id);
            }
            return Ok();
        }
    }
}