﻿using RENT.Data.Filter;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Data.Interfaces
{
    public interface IProductsRepository
    {
        public Task AddProductsAsync(ProducRequestDto product);
        public Task<List<ProductDto>> GetProductsAsync(string ImageSrc, PaginationFilter validFilter, string route);
        public Task<List<Products>> GetProductIdAsync(Guid Id);
        public Task UpdateProductAsync(ProducRequestDto productDto);
        public Task RemoveProductsAsync(string userId);
    }
}
