using RENT.Data.Filter;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Dtos.ResponseDto;
using RENT.Domain.Entities;

namespace RENT.Data.Interfaces
{
    public interface IProductsService
    {
        Task AddProductWithImage(ProducRequestDto product);
        Task<ProductResponseDto> GetAllProductsAsync(PaginationFilter filter, string ImageSrc, string route);
        Task<ProductDto> GetProductById(Guid userId, string imageSrc);
        Task UpdateItemAsync(ProducRequestDto product);
    }
}
