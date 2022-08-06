namespace RENT.Domain.Entities
{
    public class Categories
    {
        public Guid CategoriesId { get; set; }
        public string CategoriesName { get; set; }
        public string Decription { get; set; }
        public string ImageName { get; set; }
        public ICollection<Products> Products { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}