using RENT.Domain.Dtos;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Data.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> GetItemIdAsync(string Id);

        Task<UserDto> GetAllItems();

        Task AddItemAsync(T t, string UserId);

        Task RemoveItemAsync(string Id);

        Task<IEnumerable<T>> Search(string name);
        Task<RequestUserDto> UpdateItemAsync(RequestUserDto userDto);
    }
}