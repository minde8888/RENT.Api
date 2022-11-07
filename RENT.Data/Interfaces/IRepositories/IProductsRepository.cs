using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Dtos.ResponseDto;
using RENT.Domain.Entities;

namespace RENT.Data.Interfaces.IRepositories
{
    public interface IProductsRepository
    {
        public Task AddProductsAsync(ProductsRequestDto product);
        public Task<ProductResponseDto> GetProductsAsync(string imageSrc, PaginationFilter validFilter, string route);
        public Task<Products> GetProductIdAsync(Guid id);
        public Task UpdateProductAsync(ProductsRequestDto productDto);
        public Task RemoveProductsAsync(string userId);
    }
}
