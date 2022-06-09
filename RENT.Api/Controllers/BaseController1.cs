using AutoMapper;
using RENT.Data.Interfaces;
using RENT.Domain.Entities;

namespace RENT.Api.Controllers
{
    public class BaseController<T>
    {
        private IBaseRepository<BaseEntity> baseRepository;
        private IBaseSerrvice<BaseEntity> baseSerrvice;
        private IMapper mapper;
        private IWebHostEnvironment hostEnvironment;

        public BaseController(IBaseRepository<BaseEntity> baseRepository, IBaseSerrvice<BaseEntity> baseSerrvice, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            this.baseRepository = baseRepository;
            this.baseSerrvice = baseSerrvice;
            this.mapper = mapper;
            this.hostEnvironment = hostEnvironment;
        }
    }
}