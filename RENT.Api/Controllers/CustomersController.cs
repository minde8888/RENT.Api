using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos;
using RENT.Domain.Entities;
using RENT.Services.Services;
using System.Security.Claims;

namespace RENT.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : BaseController<BaseEntity>
    {
        private readonly IMapper _mapper;
        private readonly ImagesService _imagesService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IBaseRepository<BaseEntity> _baseEntity;

        public CustomersController(IMapper mapper,
            ImagesService imagesService,
            IWebHostEnvironment hostEnvironment,
            IBaseRepository<BaseEntity> baseEntity) : base(baseEntity)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _imagesService = imagesService ?? throw new ArgumentNullException(nameof(imagesService));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
            _baseEntity = baseEntity ?? throw new ArgumentNullException(nameof(baseEntity));
        }

        [HttpGet]
        //[Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<List<CustomersDto>>> GetAllEmployee()
        {
            try
            {
                var customer = new CustomersDto();
                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                return await _employeeService.GetImagesAsync(customer, ImageSrc);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error Get data from the database -> NewItem");
            }
        }

        [HttpGet("id")]
        //[Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<List<CustomersDto>>> Get(String id)
        {
            try
            {
                var userId = new Guid(id);
                if (userId == Guid.Empty)
                    return BadRequest();

                var result = await _employeeRepository.GetItemIdAsync(userId);
                if (result == null)
                    return NotFound();

                String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);

                var employeeDto = _mapper.Map<List<CustomersDto>>(result);
                _employeeService.GetImagesAsync(employeeDto, ImageSrc);

                return Ok(employeeDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Could not find web user account");
            }
        }

        [HttpPost]
        //[Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> AddNewEmployee([FromForm] RequestEmployeeDto employee)
        {
            try
            {
                if (!String.IsNullOrEmpty(employee.ImageName))
                {
                    string path = _hostEnvironment.ContentRootPath;
                    var imageName = _imagesService.SaveImage(employee.ImageFile, employee.Height, employee.Width);
                }
                string UserId = HttpContext.User.FindFirstValue("id");
                await _employeeRepository.AddEmployeeAsync(UserId, employee);

                return CreatedAtAction("Get", new { employee.Id }, employee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error Get data from the database -> AddNewEmployee");
            }
        }

    }
}
