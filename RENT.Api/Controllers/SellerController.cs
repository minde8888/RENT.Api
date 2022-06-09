using AutoMapper;
using RENT.Data.Interfaces;
using RENT.Domain.Entities;
using RENT.Services.Services;

namespace RENT.Api.Controllers
{
    public class SellerController : BaseController<BaseEntity>
    {
        public SellerController(IBaseRepository<BaseEntity> baseRepository, 
            IBaseSerrvice<BaseEntity> baseSerrvice, 
            IMapper mapper, 
            //ImagesService imagesService, 
            IWebHostEnvironment hostEnvironment) : 
            base(baseRepository, baseSerrvice, mapper, hostEnvironment)
        {
        }//, imagesService
    }
}
