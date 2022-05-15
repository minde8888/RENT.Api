using Microsoft.AspNetCore.Http;

namespace RENT.Data.Interfaces
{
    public interface ICompressimage
    {
        public void Resize(string imagePath, string imageName, IFormFile imageFile);
    }
}
