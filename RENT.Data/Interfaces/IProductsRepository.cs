using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Data.Interfaces
{
    public interface IProductsRepository
    {
        Task AddProductsAsync(ProducRequesttDto product);
        Task<List<ProductDto>> GetProductsAsync(string ImageSrc);
        Task<List<Products>> GetProductIdAsync(Guid Id);
        void UpdateProductAsync(ProducRequesttDto productDto);
        Task RemoveProductsAsync(Guid userId);
    }
}
