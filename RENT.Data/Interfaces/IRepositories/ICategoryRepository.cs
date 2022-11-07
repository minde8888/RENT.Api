using RENT.Domain.Dtos;

namespace RENT.Data.Interfaces.IRepositories
{
    public interface ICategoryRepository
    {
        public Task<CategoriesDto> AddCategoryAsync(CategoriesDto category);
        public void RemoveCategoryAsync(string id);
        public Task UpdateCategory(CategoriesDto category);
        Task<CategoriesDto> GetCategoriesIdAsync(Guid guidId);
        Task<List<CategoriesDto>> GetAllCategoriesAsync();
    }
}
