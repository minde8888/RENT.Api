
namespace RENT.Domain.Entities
{
    public class Address
    {
        public Guid AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string CompanyCode { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid ShopId { get; set; }
        public virtual Shop Shop { get; set; }
        public Guid SellerId { get; set; }
        public virtual Seller Seller { get; set; }
        public Guid CustomerId { get; set; }
        public virtual Customers Customers { get; set; }
    }
}