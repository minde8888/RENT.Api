using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RENT.Data.Interfaces.IRepositories;
using RENT.Data.Interfaces.IServices;
using RENT.Domain.Entities;

namespace RENT.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SellerController : BaseController<Seller>
    {
        public SellerController(IBaseRepository<Seller> baseRepository,
            IBaseService<Seller> baseService,
            IWebHostEnvironment hostEnvironment) :
            base(baseRepository, baseService, hostEnvironment)
        {
        }
    }
}
