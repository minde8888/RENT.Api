
namespace RENT.Data.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task AddItemAsync(T t);
        Task RemoveItemAsync(string Id);
        Task<IEnumerable<T>> Search(string name);
    }
}
