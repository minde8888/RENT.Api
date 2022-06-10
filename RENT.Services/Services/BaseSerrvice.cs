using RENT.Data.Interfaces;
using RENT.Domain.Dtos;

namespace RENT.Services.Services
{
    public class BaseSerrvice<T> : IBaseSerrvice<T>
    {
        public UserDto GetImagesAsync(UserDto userDto, string imageSrc)
        {
            var imgName = userDto.ImageName;
            userDto.ImageSrc.Add(string.Format("{0}/Images/{1}", imageSrc, imgName));
            return userDto;
        }
    }
}
