using RENT.Domain.Dtos;


namespace RENT.Data.Interfaces
{
    public interface IBaseSerrvice<T>
    {
        public UserDto GetImagesAsync(UserDto userDto, string imageSrc);
    }
}
