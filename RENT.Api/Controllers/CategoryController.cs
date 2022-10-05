using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos;
using System.Runtime.Versioning;

namespace RENT.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        [SupportedOSPlatform("windows")]
        public async Task<IActionResult> AddNewCategory(CategoriesDto category)
        {
            try
            {
                var categoryReturn = await _categoryRepository.AddCategotyAsync(category);
                return Ok(categoryReturn);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error add data to the database -> AddNewProduct");
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPut("Update")]
        [SupportedOSPlatform("windows")]
        public ActionResult UpdateCategory(CategoriesDto category)
        {
            try
            {
                _categoryRepository.UpdateCategoty(category);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error update  -> Category");
            }

        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "User, Admin")]
        public ActionResult DeleteCategory(String id)
        {
            if (String.IsNullOrEmpty(id))
                return BadRequest();

            _categoryRepository.RemoveCategoryAsync(id);
            return Ok();
        }
    }
}
