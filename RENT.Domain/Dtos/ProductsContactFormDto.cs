
namespace RENT.Domain.Dtos
{
    public class ProductsContactFormDto
    {
        public Guid ProductsContactFormId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public string Phone { get; set; }
        public bool Opened { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
