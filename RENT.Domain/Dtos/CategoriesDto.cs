using RENT.Domain.Dtos.RequestDto;

namespace RENT.Domain.Dtos
{
    public class CategoriesDto
    {
        public Guid CategoriesId { get; set; }
        public ICollection<ProductDto> Products { get; set; }
    }
}
