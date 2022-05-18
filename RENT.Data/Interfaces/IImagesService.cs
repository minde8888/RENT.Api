using Microsoft.AspNetCore.Http;

namespace RENT.Data.Interfaces
{
    public interface IImagesService
    {
        public void SaveImage(IList<IFormFile> imageFile, string height, string width);
        public void ResizeImage(string imagePath, IList<IFormFile> imageFile, int height, int width);
        public void DeleteImage(string imagePath);
    }
}
