using Microsoft.AspNetCore.Http;
using RENT.Data.Interfaces;
using System.Drawing;
using System.Runtime.Versioning;

namespace RENT.Services.Services
{
    [SupportedOSPlatform("windows")]
    public class ImagesService : IImagesService
    {
        public string SaveImage(IFormFile imageFile, string height, string width)
        {
            if (imageFile != null)
            {
                var imageName = "";

                string imageNames = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                imageNames = imageNames + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
                var imagePath = Path.Combine("Images", imageNames);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
                int heightInt = (int)Int64.Parse(height);
                int widthInt = (int)Int64.Parse(width);

                ResizeImage(imagePath, imageFile, heightInt, widthInt);
                imageName += imageNames + ",";

                return imageName;
            }
            throw new Exception();
        }

        public void ResizeImage(string imagePath, IFormFile imageFile, int height, int width)
        {
            Image image = Image.FromStream(imageFile.OpenReadStream(), true, true);
            var newImage = new Bitmap(width, height);
            using var a = Graphics.FromImage(newImage);
            a.DrawImage(image, 0, 0, width, height);
            newImage.Save(imagePath);
        }

        public void DeleteImage(string imagePath)
        {
            if (File.Exists(imagePath))
                File.Delete(imagePath);
        }
    }
}