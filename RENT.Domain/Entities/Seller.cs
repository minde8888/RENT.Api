

namespace RENT.Domain.Entities
{
    public class Seller : BaseEntity
    {
        public Guid AddressId { get; set; }
        public Guid ProductsId { get; set; }
        public Guid CustomersId { get; set; }
    }
}
