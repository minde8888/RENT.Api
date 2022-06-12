using AutoMapper;
using RENT.Data.Interfaces;
using RENT.Domain.Entities;
using RENT.Services.Services;

namespace RENT.Api.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SellerController : BaseController<Seller>
    {
        public SellerController(IBaseRepository<Seller> baseRepository,
            IBaseSerrvice<Seller> baseSerrvice,
            IMapper mapper,
            IImagesService imagesService,
            IWebHostEnvironment hostEnvironment) :
            base(baseRepository, baseSerrvice, mapper, imagesService, hostEnvironment)
        {
        }
    }
}
