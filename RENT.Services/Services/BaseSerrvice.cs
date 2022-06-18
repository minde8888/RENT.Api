using RENT.Data.Interfaces;
using RENT.Domain.Dtos;

namespace RENT.Services.Services
{
    public class BaseSerrvice<T> : IBaseSerrvice<T>
    {
        public Task<UserDto>  GetImagesAsync(UserDto userDto, string imageSrc)
        {
            List<string> imageList = new();

            if (!string.IsNullOrEmpty(userDto.ImageName))

                imageList.Add(string.Format("{0}/Images/{1}", imageSrc, userDto.ImageName));

            userDto.ImageSrc = imageList;

            return Task.FromResult(userDto);
        }
    }
}

