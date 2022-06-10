using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RENT.Data.Interfaces;
using RENT.Domain.Entities;
using RENT.Services.Services;

namespace RENT.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SellerController : BaseController<BaseEntity>
    {
        public SellerController(IBaseRepository<BaseEntity> baseRepository,
            IBaseSerrvice<BaseEntity> baseSerrvice,
            IMapper mapper,
            ImagesService imagesService,
            IWebHostEnvironment hostEnvironment) :
            base(baseRepository, baseSerrvice, mapper, imagesService, hostEnvironment)
        {
        }

    }
}
