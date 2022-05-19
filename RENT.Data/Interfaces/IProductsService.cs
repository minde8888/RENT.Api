using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Data.Interfaces
{
    public interface IProductsService
    {
        List<ProductDto> GetProductImageAsync(List<Products> products, string imageSrc);
    }
}
