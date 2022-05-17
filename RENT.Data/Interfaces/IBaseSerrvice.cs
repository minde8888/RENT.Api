using RENT.Domain.Dtos;


namespace RENT.Data.Interfaces
{
    public interface IBaseSerrvice<T>
    {
        public List<T> GetImagesAsync(List<T> baseDto, string imageSrc);
    }
}
