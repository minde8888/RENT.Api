using RENT.Domain.Dtos;
using RENT.Domain.Dtos.RequestDto;

namespace RENT.Data.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<List<T>> GetItemIdAsync(string Id);

        //Task<List<UserDto>> GetAllItems();

        //Task AddItemAsync(T t, string UserId);

        //Task RemoveItemAsync(string Id);

        //Task<IEnumerable<T>> Search(string name);
        //Task<RequestUserDto> UpdateItemAsync(RequestUserDto userDto);
    }
}