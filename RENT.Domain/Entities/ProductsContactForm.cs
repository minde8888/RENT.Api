namespace RENT.Domain.Entities
{
    public class ProductsContactForm
    {
        public Guid ProductsContactFormId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public string Phone { get; set; }
        public bool Opened { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public Guid? ProductsId { get; set; }
        public virtual Products Products { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
