using Microsoft.AspNetCore.Http;
using RENT.Data.Interfaces;
using System.Drawing;
using System.Runtime.Versioning;

namespace RENT.Services.Services
{
    [SupportedOSPlatform("windows")]
    public class ImagesService : IImagesService
    {
        public string SaveImage(IList<IFormFile> imageFile, IList<string> height, IList<string> width)
        {
            if (imageFile != null)
            {
                var imaneName = "";

                for (int i = 0; i < imageFile.Count; i++)
                {
                    string imageName = new String(Path.GetFileNameWithoutExtension(imageFile[i].FileName).Take(10).ToArray()).Replace(' ', '-');
                    imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile[i].FileName);
                    var imagePath = Path.Combine("Images", imageName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        imageFile[i].CopyTo(stream);
                    }
                    int heightInt = (int)Int64.Parse(height[i]);
                    int widthInt = (int)Int64.Parse(width[i]);

                    ResizeImage(imagePath, imageFile, heightInt, widthInt);
                    imaneName += imageName+",";
                }
                return imaneName;
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