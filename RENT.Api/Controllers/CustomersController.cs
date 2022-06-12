using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RENT.Data.Interfaces;
using RENT.Domain.Entities;
using RENT.Services.Services;

namespace RENT.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : BaseController<Customers>
    {
        public CustomersController(IBaseRepository<Customers> baseRepository,
            IBaseSerrvice<Customers> baseSerrvice,
            IMapper mapper,
            IImagesService imagesService,
            IWebHostEnvironment hostEnvironment) :
            base(baseRepository, baseSerrvice, mapper, imagesService, hostEnvironment)
        {

        }
    }
}
