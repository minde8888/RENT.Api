using System;

namespace RENT.Domain.Dtos
{
    public class AddressDto
    {
        public Guid AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string CompanyCode { get; set; }
        public Guid? ShopId { get; set; }
        public ShopDto Shop { get; set; }
        public Guid? SellerId { get; set; }
        public SellerDto Seller { get; set; }
        public Guid? CustomerId { get; set; }
        public CustomersDto Customers { get; set; }
    }
}
