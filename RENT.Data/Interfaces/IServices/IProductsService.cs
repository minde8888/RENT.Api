using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Dtos.ResponseDto;
using RENT.Domain.Entities;

namespace RENT.Data.Interfaces.IServices
{
    public interface IProductsService
    {
        Task AddProductWithImage(ProductsRequestDto product);
        Task<ProductResponseDto> GetAllProductsAsync(PaginationFilter filter, string imageSrc, string route);
        Task<ProductsDto> GetProductById(Guid userId, string imageSrc);
        Task<ProductsDto> UpdateItemAsync(ProductsRequestDto product, string imageSrc);
    }
}
