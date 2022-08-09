using RENT.Domain.Dtos.RequestDto;

namespace RENT.Domain.Dtos
{
    public class CategoriesDto
    {
        public Guid CategoriesId { get; set; }
        public string CategoriesName { get; set; }
        public string Decription { get; set; }
        public string ImageName { get; set; }
    }
}
