using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Data.Interfaces
{
    public interface IProductsRepository
    {
        public Task AddProductsAsync(ProducRequesttDto product);
        public Task<List<ProductDto>> GetProductsAsync(string ImageSrc);
        public Task<List<Products>> GetProductIdAsync(Guid Id);
        public Task UpdateProductAsync(ProducRequesttDto productDto);
        public Task RemoveProductsAsync(string userId);
    }
}
