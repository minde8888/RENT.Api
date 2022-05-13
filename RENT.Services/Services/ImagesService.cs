using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;

namespace WTP.Services.Services
{
    [SupportedOSPlatform("windows")]
    public class ImagesService
    {
        public string SaveImage(IFormFile imageFile, string height, string width )
        {
            if (imageFile != null)
            {
                string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
                var imagePath = Path.Combine( "Images", imageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
                int heightInt = (int)Int64.Parse(height);
                int widthInt = (int)Int64.Parse(width);

                ResizeImage(imagePath, imageFile, heightInt, widthInt);

                return imageName;
            }
            throw new Exception();
        }
        private void ResizeImage(string imagePath, IFormFile imageFile, int height, int width)
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