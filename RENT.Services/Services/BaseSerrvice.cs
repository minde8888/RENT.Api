using RENT.Data.Interfaces;
using RENT.Domain.Dtos;
using RENT.Domain.Entities;

namespace RENT.Services.Services
{
    public class BaseSerrvice<T> : IBaseSerrvice<T> where T : BaseEntity
    {
        public List<T> GetImagesAsync(List<T> t, string imageSrc)
        {

            foreach (var baseImage in t)
            {
                //foreach (var img in baseImage.ImageName)
                //{
                //    baseImage.ImageSrc.Add(String.Format("{0}/Images/{1}", imageSrc, img));
                //}
            }
            return t;
        }
    }
}
