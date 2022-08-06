﻿using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Data.Interfaces
{
    public interface IProductsRepository
    {
        Task<List<ProductDto>> AddProductsAsync(ProducRequesttDto product);
        Task<List<ProductDto>> GetProductsAsync(string ImageSrc);
        Task<List<Products>> GetProductIdAsync(Guid Id);
        Task<IList<ProductDto>> UpdateProductAsync(ProductDto productDto);
        Task RemoveProductsAsync(Guid userId);
    }
}
