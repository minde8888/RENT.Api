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
        private readonly IProductsRepository _productsRepository;
        private readonly IProductsService _productsService;


        public ProductsController(

            IProductsService productsService,
            IProductsRepository productsRepository)
        {

            _productsRepository = productsRepository;
            _productsService = productsService;
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        [SupportedOSPlatform("windows")]
        public async Task<ActionResult> AddNewProductAsync([FromForm] ProducRequestDto product)
        {
            try
            {
                await _productsService.AddProductWithImage(product);
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
                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                var response = await _productsService.GetAllProductsAsync(filter, ImageSrc, route);
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

                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                var productsToReturn = await _productsService.GetProductById(userId, ImageSrc);
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
        public ActionResult UpdateAsync([FromForm] ProducRequestDto product)
        {
            if (product.ProductsId == Guid.Empty)
                return BadRequest("This product can not by updated");
            try
            {
                _productsService.UpdateItemAsync(product);
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