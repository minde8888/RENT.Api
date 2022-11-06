using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RENT.Data.Interfaces.IRepositories;
using RENT.Data.Interfaces.IServices;
using RENT.Domain.Entities;

namespace RENT.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : BaseController<Customers>
    {
        public CustomersController(IBaseRepository<Customers> baseRepository,
            IBaseSerrvice<Customers> baseSerrvice,
            IWebHostEnvironment hostEnvironment) :
            base(baseRepository, baseSerrvice, hostEnvironment)
        {

        }
    }
}
