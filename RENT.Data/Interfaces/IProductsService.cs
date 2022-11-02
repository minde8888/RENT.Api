using RENT.Data.Filter;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Dtos.ResponseDto;

namespace RENT.Data.Interfaces
{
    public interface IProductsService
    {
        Task AddProductWithImage(ProductsRequestDto product);
        Task<ProductResponseDto> GetAllProductsAsync(PaginationFilter filter, string ImageSrc, string route);
        Task<ProductsDto> GetProductById(Guid userId, string imageSrc);
        Task<ProductsDto> UpdateItemAsync(ProductsRequestDto product, string imageSrc);
    }
}
