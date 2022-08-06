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
                //if (product.Images != null)
                //{
                //    string path = _hostEnvironment.ContentRootPath;
                //    foreach (var item in product.Images)
                //    {
                //        _imagesService.SaveImage(item, product.ImageHeight, product.ImageWidth);
                //    }
                //}
                var data = await _productsRepository.AddProductsAsync(product);
                return Ok(data);
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error Get data from the database");
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
        public async Task<ActionResult<IList<ProductDto>>> Update([FromForm] ProductDto product)
        {
            if (product.ProductsId == Guid.Empty)
                return BadRequest("This project can not by updated");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var productToReturn = await _productsRepository.UpdateProductAsync(product);

                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);

                //foreach (var item in productToReturn)
                //{
                //    if (!item.Attachments.Any() && !item.ImageName.Any())
                //    {
                //        string[] imagesNames = item.ImageName.Split(',');
                //        foreach (var ImageName in imagesNames)
                //        {
                //            string imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", ImageName);
                //            _imagesService.DeleteImage(imagePath);

                //            //item.ImageName =  _imagesService.SaveImage(product.Attachments, product.Height, product.Width);

                //            item.ImageSrc.Add(String.Format("{0}/Images/{1}", ImageSrc, ImageName));
                //        }
                //    }
                //}
                return Ok(productToReturn);
            }
            catch (DbUpdateConcurrencyException)
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