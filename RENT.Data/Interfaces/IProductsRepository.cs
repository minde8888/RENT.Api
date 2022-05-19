using Microsoft.AspNetCore.Mvc;
using RENT.Domain.Dtos.RequestDto;

namespace RENT.Data.Interfaces
{
    public interface IProductsRepository
    {
        Task AddProductsAsync(RequestProductsDto product);
    }
}
