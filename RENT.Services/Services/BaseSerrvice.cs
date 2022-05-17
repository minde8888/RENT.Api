using RENT.Data.Interfaces;
using RENT.Domain.Dtos;

namespace RENT.Services.Services
{
    public class BaseSerrvice<T>: IBaseSerrvice<T> where T : BaseEntityDto
    {
        public List<T> GetImagesAsync(List<T> t, string imageSrc)
        {
            foreach (var baseImage in t)
            {
                string baseImageName = baseImage.ImageName;
                baseImage.ImageSrc = String.Format("{0}/Images/{1}", imageSrc, baseImageName);
            }
            return t;
        }
    }
}
