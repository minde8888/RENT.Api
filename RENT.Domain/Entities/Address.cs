using RENT.Domain.Entities;

namespace WTP.Domain.Entities
{
    public class Address
    {
        public Guid? AddressId { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
        public string? CompanyCode { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageName { get; set; }
        public Guid? ShopId { get; set; }
        public Shop? Shop { get; set; }
        public Guid? SellerId { get; set; }
        public Seller? Seller { get; set; }
        public Guid? CustomerId { get; set; }
        public Customers? Customers { get; set; }
    }
}
