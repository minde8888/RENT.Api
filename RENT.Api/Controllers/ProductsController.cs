using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RENT.Api.Controllers
{
    public class ProductsController
    {

        //[HttpPost]
        //[Authorize(Roles = "Seller, Admin")]
        //public async Task<IActionResult> AddNewEmployee([FromForm] RequestEmployeeDto employee)
        //{
        //    try
        //    {
        //        if (!String.IsNullOrEmpty(employee.ImageName))
        //        {
        //            string path = _hostEnvironment.ContentRootPath;
        //            var imageName = _imagesService.SaveImage(employee.ImageFile, employee.Height, employee.Width);
        //        }
        //        string UserId = HttpContext.User.FindFirstValue("id");
        //        await _employeeRepository.AddEmployeeAsync(UserId, employee);

        //        return CreatedAtAction("Get", new { employee.Id }, employee);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //        "Error Get data from the database -> AddNewEmployee");
        //    }
        //}

    }
}
