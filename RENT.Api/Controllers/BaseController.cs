using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RENT.Data.Interfaces;
using RENT.Domain.Entities;
using RENT.Services.Services;

namespace RENT.Api.Controllers
{
    public class BaseController<T> : ControllerBase where T : BaseEntity
    {
        private readonly IBaseRepository<T> _baseRepository;
        private IBaseRepository<BaseEntity> baseEntity;

        //private readonly _context;

        public BaseController(IBaseRepository<T> itemServices, IMapper mapper, ImagesService imagesService, IWebHostEnvironment hostEnvironment)
        {
            _baseRepository = itemServices;
        }

        public BaseController(IBaseRepository<BaseEntity> baseEntity)
        {
            this.baseEntity = baseEntity;
        }

        [HttpGet("Search")]
        //[Authorize(Roles = "Manager, Admin, Employee")]
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
