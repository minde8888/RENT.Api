using Microsoft.AspNetCore.Http;

namespace RENT.Data.Interfaces.IServices
{
    public interface IImagesService
    {
        public string SaveImage(IFormFile imageFile, string height, string width);
        public void ResizeImage(string imagePath, IFormFile imageFile, int height, int width);
        public void DeleteImage(string imagePath);
    }
}
