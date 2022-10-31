using RENT.Domain.Dtos;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Data.Interfaces
{
    public interface IBaseSerrvice<T>
    {
        UserDto GetImagesAsync(UserDto userDto, string imageSrc);
        Task<UserDto> GetItemById(string imageSrc, string id);
        Task<UserDto> UpdateItem(string contentRootPath, RequestUserDto userDto, string src);
    }
}
