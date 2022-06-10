using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos;
using RENT.Domain.Entities;
using RENT.Services.Services;
using System.Security.Claims;

namespace RENT.Api.Controllers
{

    [Route("api/v1/[controller]")]
    public class BaseController<T> : ControllerBase where T : BaseEntity
    {
        private readonly IBaseRepository<T> _baseRepository;
        private readonly IBaseSerrvice<T> _baseSerrvice;
        private readonly IMapper _mapper;
        private readonly IImagesService _imagesService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BaseController(IBaseRepository<T> baseRepository,
            IBaseSerrvice<T> baseSerrvice,
            IMapper mapper,
            ImagesService imagesService,
            IWebHostEnvironment hostEnvironment)
        {
            _baseRepository = baseRepository;
            _baseSerrvice = baseSerrvice;
            _mapper = mapper;
            _imagesService = imagesService;
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewItem([FromForm] T t)
        {
            try
            {
                string UserId = HttpContext.User.FindFirstValue("id");
                await _baseRepository.AddItemAsync(t, UserId);
                return CreatedAtAction("Get", new { t.Id }, t);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error Add data to the database -> AddNewItem");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            try
            {
                return await _baseRepository.GetAllItems();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error Get project data from the database");
            }
        }

        [HttpGet("id")]
        public async Task<ActionResult<T>> Get(String id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest();

                var item = await _baseRepository.GetItemIdAsync(id);
                if (item == null)
                    return NotFound();

                var result = _mapper.Map<UserDto>(item);
                var newAddress = _mapper.Map<AddressDto>(item.Address);
                result.AddressDto = newAddress;

                if (result.ImageName != null)
                {
                    String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                    return Ok(_baseSerrvice.GetImagesAsync(result, ImageSrc));
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error Get by id data from the database" + ex);
            }
        }


        //[HttpPut("Update/{id}")]
        //[Authorize(Roles = "Admin, Employee")]
        //[SupportedOSPlatform("windows")]
        //public async Task<ActionResult<List<ResponseUserDto>>> UpdateAddressAsync(string id, [FromForm] RequestUserDto userDto)
        //{
        //    try
        //    {
        //        if (id == null)
        //            return BadRequest("This user can not by updated");

        //        userDto.Id = new Guid(id);

        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);

        //        if (userDto.ImageFile != null && userDto.ImageName != null)
        //        {
        //            string[] imagesNames = userDto.ImageName.Split(',');
        //            foreach (var image in imagesNames)
        //            {
        //                string imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", image);
        //                _imagesService.DeleteImage(imagePath);
        //            }

        //            userDto.ImageName = _imagesService.SaveImage(userDto.ImageFile, userDto.Height, userDto.Width);
        //        }

        //        var item = await _baseRepository.UpdateItemAsync(userDto);

        //        String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
        //        foreach (var img in item.ImageName)
        //        {
        //            item.ImageSrc.Add(String.Format("{0}/Images/{1}", ImageSrc, img));
        //        }


        //        return Ok(item);
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //           "Error save DB");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //           ex);
        //    }
        //}

        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<T>>> Search(string name)
        {
            try
            {
                var result = await _baseRepository.Search(name);

                if (result.Any())
                    return Ok(result);

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult> DeleteManager(String id)
        {
            if (id == String.Empty)
                return BadRequest();

            await _baseRepository.RemoveItemAsync(id);
            return Ok();
        }
    }
}