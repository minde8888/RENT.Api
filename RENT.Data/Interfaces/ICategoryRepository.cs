using RENT.Domain.Dtos;

namespace RENT.Data.Interfaces
{
    public interface ICategoryRepository
    {
        public Task AddCategotyAsync(CategoriesDto category);
        public void RemoveCategoryAsync(string id);
    }
}
