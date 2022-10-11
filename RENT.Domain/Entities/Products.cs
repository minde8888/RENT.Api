namespace RENT.Domain.Entities
{
    public class Products
    {
        public Guid ProductsId { get; set; }
        public string ImageHeight { get; set; }
        public string ImageWidth { get; set; }
        public string ImageName { get; set; }
        public string Price { get; set; }
        public string Size { get; set; }
        public string Place { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid SellerId { get; set; }
        public Seller Seller { get; set; }
        public Customers Customers { get; set; }
        public Guid PostsId { get; set; }
        public Posts Posts { get; set; }
        public Guid? ProductsContactFormId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; }
        public ICollection<Categories> Categories { get; set; }
    }
}