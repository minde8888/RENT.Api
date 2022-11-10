using RENT.Data.Interfaces.IRepositories;
using RENT.Data.Interfaces.IServices;
using RENT.Domain.Dtos;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;

namespace RENT.Services.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        private readonly IBaseRepository<T> _baseRepository;
        private readonly IImagesService _imagesService;


        public BaseService(IBaseRepository<T> baseRepository, IImagesService imagesService)
        {
            _baseRepository = baseRepository ?? throw new ArgumentNullException(nameof(baseRepository));
            _imagesService = imagesService ?? throw new ArgumentNullException(nameof(imagesService));
        }

        public async Task<UserDto> GetItemById(string imageSrc, string id)
        {
            var user = await _baseRepository.GetItemIdAsync(id);
            if (user.ImageName != null)
            {
                GetImages(user, imageSrc);
            }
            return user;
        }
        public async Task<UserDto> UpdateItem(string contentRootPath, RequestUserDto user, string src)
        {
            if (user.ImageFile != null && user.ImageName != null)
            {
                var imagesNames = user.ImageName.Split(',');
                foreach (var image in imagesNames)
                {
                    var imagePath = Path.Combine(contentRootPath, "Images", image);
                    _imagesService.DeleteImage(imagePath);
                }
                user.ImageName = _imagesService.SaveImage(user.ImageFile, user.Height, user.Width);
            }
            var itemReturn = await _baseRepository.UpdateItemAsync(user);
            GetImages(itemReturn, src);

            return itemReturn;
        }
        public UserDto GetImages(UserDto userDto, string imageSrc)
        {
            List<string> imageList = new();

            if (!string.IsNullOrEmpty(userDto.ImageName))
                imageList.Add($"{imageSrc}/Images/{userDto.ImageName}");

            userDto.ImageSrc = imageList;

            return userDto;
        }
    }
}

