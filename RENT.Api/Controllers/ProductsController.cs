namespace RENT.Api.Controllers
{
    public class ProductsController
    {

        //[HttpPost]
        //[Authorize(Roles = "Seller, Admin")]
        //public async Task<IActionResult> AddNewproduct([FromForm] RequestProductsDto product)
        //{
        //    try
        //    {
        //        if (!String.IsNullOrEmpty(product.ImageName))
        //        {
        //            string path = _hostEnvironment.ContentRootPath;
        //            var imageName = _imagesService.SaveImage(product.ImageFile, product.Height, product.Width);
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
