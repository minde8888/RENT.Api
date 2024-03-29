﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RENT.Data.Interfaces.IRepositories;
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
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        [HttpPost]
        [SupportedOSPlatform("windows")]
        public async Task<IActionResult> AddNewCategory(CategoriesDto category)
        {
            try
            {
                var categoryReturn = await _categoryRepository.AddCategoryAsync(category);
                return Ok(categoryReturn);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error add data to the database -> AddNewProduct");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<CategoriesDto>> GetAll()
        {
            try
            {
                var productsInCategory = await _categoryRepository.GetAllCategoriesAsync();
                if (productsInCategory == null)
                    return NotFound();

                return Ok(productsInCategory);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Could not find categories");
            }
        }

        [HttpGet("id")]
        [AllowAnonymous]
        public async Task<ActionResult<CategoriesDto>> Get(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("Could not find id");
                var guidId = new Guid(id);

                var productsInCategory = await _categoryRepository.GetCategoriesIdAsync(guidId);
                if (productsInCategory == null)
                    return NotFound();

                return Ok(productsInCategory);
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
        public ActionResult Update(CategoriesDto category)
        {
            try
            {
                _categoryRepository.UpdateCategory(category);
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
        public ActionResult Delete(string id)
        {
            if (String.IsNullOrEmpty(id))
                return BadRequest();

            _categoryRepository.RemoveCategoryAsync(id);
            return Ok();
        }
    }
}