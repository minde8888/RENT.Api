

namespace RENT.Domain.Entities
{
    public class Seller : BaseEntity
    {
        public Guid SellerId { get; set; }
        public Guid AddressId { get; set; }
        public Guid GoodsId { get; set; }
        public Guid CustomersId { get; set; }
    }
}
