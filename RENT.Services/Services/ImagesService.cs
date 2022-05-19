using Microsoft.AspNetCore.Http;
using RENT.Data.Interfaces;
using System.Drawing;
using System.Runtime.Versioning;

namespace RENT.Services.Services
{
    [SupportedOSPlatform("windows")]
    public class ImagesService : IImagesService
    {
        public IList<string> SaveImage(IList<IFormFile> imageFile, string height, string width)
        {
            if (imageFile != null)
            {
                IList<string> list = new List<string>();

                foreach (IFormFile file in imageFile)
                {
                    string imageName = new String(Path.GetFileNameWithoutExtension(file.FileName).Take(10).ToArray()).Replace(' ', '-');
                    imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(file.FileName);
                    var imagePath = Path.Combine("Images", imageName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    int heightInt = (int)Int64.Parse(height);
                    int widthInt = (int)Int64.Parse(width);

                    ResizeImage(imagePath, imageFile, heightInt, widthInt);
                    list.Add(imageName);
                }
                return list;
            }

            throw new Exception();
        }

        public void ResizeImage(string imagePath, IList<IFormFile> imageFile, int height, int width)
        {
            foreach (IFormFile file in imageFile)
            {
                Image image = Image.FromStream(file.OpenReadStream(), true, true);
                var newImage = new Bitmap(width, height);
                using var a = Graphics.FromImage(newImage);
                a.DrawImage(image, 0, 0, width, height);
                newImage.Save(imagePath);
            }
        }

        public void DeleteImage(string imagePath)
        {
            if (File.Exists(imagePath))
                File.Delete(imagePath);
        }
    }
}