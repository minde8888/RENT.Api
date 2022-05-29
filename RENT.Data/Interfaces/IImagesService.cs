using Microsoft.AspNetCore.Http;

namespace RENT.Data.Interfaces
{
    public interface IImagesService
    {
        public string SaveImage(IList<IFormFile> imageFile, IList<string> height, IList<string> width);
        public void ResizeImage(string imagePath, IList<IFormFile> imageFile, int height, int width);
        public void DeleteImage(string imagePath);
    }
}
