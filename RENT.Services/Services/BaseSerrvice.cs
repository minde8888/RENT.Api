using RENT.Data.Interfaces;
using RENT.Data.Repositorys;
using RENT.Domain.Dtos;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Services.Services
{
    public class BaseSerrvice<T> : IBaseSerrvice<T> where T : BaseEntity
    {
        private readonly IBaseRepository<T> _baseRepository;
        private readonly IImagesService _imagesService;


        public BaseSerrvice(IBaseRepository<T> baseRepository, IImagesService imagesService)
        {
            _baseRepository = baseRepository;
            _imagesService = imagesService;
        }
    
        public async Task<UserDto> GetItemById(string imageSrc, string id) 
        {
            var user = await _baseRepository.GetItemIdAsync(id);
            if (user.ImageName != null)
            {
                GetImagesAsync(user, imageSrc);
            }
            return user; 
        }
        public async Task<UserDto> UpdateItem(string contentRootPath, RequestUserDto user, string src)
        {
            if (user.ImageFile != null && user.ImageName != null)
            {
                string[] imagesNames = user.ImageName.Split(',');
                foreach (var image in imagesNames)
                {
                    string imagePath = Path.Combine(contentRootPath, "Images", image);
                    _imagesService.DeleteImage(imagePath);
                }
                user.ImageName = _imagesService.SaveImage(user.ImageFile, user.Height, user.Width);
            }          
            var itemReturn = await _baseRepository.UpdateItemAsync(user);
            GetImagesAsync(itemReturn, src);

            return itemReturn;
        }
        public UserDto  GetImagesAsync(UserDto userDto, string imageSrc)
        {
            List<string> imageList = new();

            if (!string.IsNullOrEmpty(userDto.ImageName))

                imageList.Add(string.Format("{0}/Images/{1}", imageSrc, userDto.ImageName));

            userDto.ImageSrc = imageList;

            return userDto;
        }
    }
}

