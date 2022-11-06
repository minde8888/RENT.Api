using RENT.Domain.Dtos;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Data.Interfaces.IServices
{
    public interface IBaseSerrvice<T>
    {
        UserDto GetImages(UserDto userDto, string imageSrc);
        Task<UserDto> GetItemById(string imageSrc, string id);
        Task<UserDto> UpdateItem(string contentRootPath, RequestUserDto userDto, string src);
    }
}
