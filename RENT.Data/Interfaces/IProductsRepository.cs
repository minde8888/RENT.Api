using Microsoft.AspNetCore.Mvc;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Dtos.ResponseDto;

namespace RENT.Data.Interfaces
{
    public interface IProductsRepository
    {
        Task<ActionResult<ResponseProductsDto>> AddNewproduct([FromForm] RequestProductsDto product);
    }
}
