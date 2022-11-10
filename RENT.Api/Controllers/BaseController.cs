using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RENT.Data.Interfaces.IRepositories;
using RENT.Data.Interfaces.IServices;
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
        private readonly IBaseService<T> _baseService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BaseController(
            IBaseRepository<T> baseRepository,
            IBaseService<T> baseService,
            IWebHostEnvironment hostEnvironment)
        {
            _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
            _baseService = baseService ?? throw new ArgumentNullException(nameof(baseService));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewItem([FromForm] T t)
        {
            try
            {
                var userId = HttpContext.User.FindFirstValue("id");
                await _baseRepository.AddItemAsync(t, userId);
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
        public async Task<ActionResult<T>> Get(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("Can not find Id");

                var imageSrc = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

                var result = await _baseService.GetItemById(imageSrc, id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error Get by id data from the database" + ex);
            }
        }

        [HttpPut("Update")]
        [Authorize(Roles = "Admin, User")]
        [SupportedOSPlatform("windows")]
        public async Task<ActionResult<List<UserDto>>> UpdateUserAsync([FromForm] RequestUserDto userDto)
        {
            try
            {
                var src = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                var result = await _baseService.UpdateItem(_hostEnvironment.ContentRootPath, userDto, src);
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
        public async Task<ActionResult> DeleteManager(string id)
        {
            if (id == string.Empty)
                return BadRequest();

            await _baseRepository.RemoveItemAsync(id);
            return Ok();
        }
    }
}