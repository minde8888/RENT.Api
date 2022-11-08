using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RENT.Data.Interfaces.IRepositories;
using RENT.Data.Interfaces.IServices;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Dtos.ResponseDto;
using RENT.Domain.Entities;
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
        public async Task<ActionResult> AddNewProductAsync([FromForm] ProductsRequestDto product)
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
                var imageSrc = $"{Request.Scheme}://{Request.Host}";
                var response = await _productsService.GetAllProductsAsync(filter, imageSrc, route);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error Get data from the database" + ex);
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPut("Update")]
        [SupportedOSPlatform("windows")]
        public async Task<ActionResult<List<ProductsDto>>> UpdateAsync([FromForm] ProductsRequestDto product)
        {
            if (product.ProductsId == Guid.Empty)
                return BadRequest("This product can not by updated");
            try
            {
                var imageSrc = $"{Request.Scheme}://{Request.Host}";
                var response = await _productsService.UpdateItemAsync(product, imageSrc);
                return Ok(response);
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
        public async Task<ActionResult> DeleteProductAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            await _productsRepository.RemoveProductsAsync(id);
            return Ok();
        }
    }
}