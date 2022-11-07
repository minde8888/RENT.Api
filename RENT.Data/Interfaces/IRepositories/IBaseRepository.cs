using RENT.Domain.Dtos;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Data.Interfaces.IRepositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<UserDto> GetItemIdAsync(string id);
        Task<UserDto> GetAllItems();
        Task AddItemAsync(T t, string userId);
        Task RemoveItemAsync(string id);
        Task<IEnumerable<T>> Search(string name);
        Task<UserDto> UpdateItemAsync(RequestUserDto userDto);
    }
}