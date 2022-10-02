namespace RENT.Domain.Dtos
{
    public class CategoriesDto
    {
        public Guid CategoriesId { get; set; }
        public string CategoriesName { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public Guid ProductsId { get; set; }
    }
}
