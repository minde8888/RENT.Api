namespace RENT.Domain.Entities
{
    public class Products
    {
        public Guid ProductsId { get; set; }
        public string ProductName { get; set; }
        public int QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        public Guid SellerId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Categories> Categories { get; set; }
    }
}