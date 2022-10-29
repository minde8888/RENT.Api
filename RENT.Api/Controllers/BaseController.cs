using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;
using System.Runtime.Versioning;
using System.Security.Claims;

namespace RENT.Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class BaseController<T> : ControllerBase where T : BaseEntity
    {
        private readonly IBaseRepository<T> _baseRepository;
        private readonly IBaseSerrvice<T> _baseSerrvice;
        private readonly IMapper _mapper;

        private readonly IWebHostEnvironment _hostEnvironment;

        public BaseController(IBaseRepository<T> baseRepository,
            IBaseSerrvice<T> baseSerrvice,
            IMapper mapper,
            IImagesService imagesService,
            IWebHostEnvironment hostEnvironment)
        {
            _baseRepository = baseRepository;
            _baseSerrvice = baseSerrvice;
            _mapper = mapper;

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
        public async Task<ActionResult<UserDto>> GetAll()
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
                    return BadRequest("Can not find Id");

                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);

                var result = await _baseSerrvice.GetItemById(ImageSrc, id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error Get by id data from the database" + ex);
            }
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin, User")]
        [SupportedOSPlatform("windows")]
        public async Task<ActionResult<List<UserDto>>> UpdateUserAsync(string id, [FromForm] RequestUserDto userDto)
        {
            try
            {
                if (id == null)
                    return BadRequest("This user can not by updated");

                String src = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);

                var result = await _baseSerrvice.UpdateItem(_hostEnvironment.ContentRootPath, userDto, src);
                return Ok(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error save DB");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   ex);
            }
        }

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
        [Authorize(Roles = "Seller, Admin")]
        public async Task<ActionResult> DeleteManager(String id)
        {
            if (id == String.Empty)
                return BadRequest();

            await _baseRepository.RemoveItemAsync(id);
            return Ok();
        }
    }
}