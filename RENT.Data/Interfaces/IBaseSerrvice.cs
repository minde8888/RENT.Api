using RENT.Domain.Dtos;


namespace RENT.Data.Interfaces
{
    public interface IBaseSerrvice<T>
    {
        Task<UserDto> GetImagesAsync(UserDto userDto, string imageSrc);
    }
}
