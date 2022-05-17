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
        public CustomersController(IBaseRepository<BaseEntity> baseRepository, 
            IBaseSerrvice<BaseEntity> baseSerrvice, 
            IMapper mapper, 
            ImagesService imagesService, 
            IWebHostEnvironment hostEnvironment) : 
            base(baseRepository, baseSerrvice, mapper, imagesService, hostEnvironment)
        {

        }
    }
}
