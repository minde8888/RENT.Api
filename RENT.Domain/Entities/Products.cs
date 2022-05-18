namespace RENT.Domain.Entities
{
    public class Products
    {
        public Guid ProductsId { get; set; }
        public string ProductName { get; set; }
        public int QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public string ImageName { get; set; }
        public string ProductNumber { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Categories> Categories { get; set; }
        public Seller Seller { get; set; }
        public Customers Customers { get; set; }
        public Posts Posts { get; set; }
        public ProductsSpecifications Specifications { get; set; }

    }
}