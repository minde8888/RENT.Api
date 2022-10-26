using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RENT.Data.Filter;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Dtos.ResponseDto;
using System.Runtime.Versioning;

namespace RENT.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IImagesService _imagesService;
        private readonly IProductsRepository _productsRepository;
        private readonly IProductsService _productsService;
        private readonly IMapper _mapper;


        public ProductsController(
            IImagesService imagesService,
            IWebHostEnvironment hostEnvironment,
            IProductsService productsService,
            IProductsRepository productsRepository,
            IMapper mapper)
        {
            _imagesService = imagesService;
            _hostEnvironment = hostEnvironment;
            _productsRepository = productsRepository;
            _productsService = productsService;
            _mapper = mapper;
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        [SupportedOSPlatform("windows")]
        public async Task<IActionResult> AddNewProductAsync([FromForm] ProducRequestDto product)
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error Get data from the database -> AddNewProduct" + ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ProductResponseDto>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            try
            {
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                var response = await _productsRepository.GetProductsAsync(ImageSrc, validFilter, route);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error Get data from the database" + ex);
            }
        }

        [HttpGet("id")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ProductDto>>> GetAsync(String id)
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

                var productsToReturn = _productsService.GetProductImage(product, ImageSrc);

                return Ok(productsToReturn);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Could not find web user account");
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPut("Update")]
        [SupportedOSPlatform("windows")]
        public async Task<ActionResult> UpdateAsync([FromForm] ProducRequestDto product)
        {
            if (product.ProductsId == Guid.Empty)
                return BadRequest("This product can not by updated");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                if (product.Images != null)
                {
                    string[] height = product.ImageHeight.Split(',');
                    string[] width = product.ImageWidth.Split(',');
                    string[] imageName = product.ImageSrc.Split(',');

                    int count = 0;

                    for (int i = 0; i < imageName.Length; i++)
                    {
                        if (imageName[i] == "/")
                        {
                            imageName[i] = _imagesService.SaveImage(product.Images[count], height[count], width[count]).Replace(",", "");
                            count++;
                        }
                    }
                    product.ImageSrc = String.Join(',', imageName);
                }

                await _productsRepository.UpdateProductAsync(product);
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

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<ActionResult> DeleteProductAsync(String id)
        {
            if (String.IsNullOrEmpty(id))
                return BadRequest();

            await _productsRepository.RemoveProductsAsync(id);
            return Ok();
        }
    }
}