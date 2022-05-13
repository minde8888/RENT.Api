using WTP.Domain.Entities;

namespace RENT.Domain.Entities
{
    public class Customers : BaseEntity
    {
        public Guid CustomersId { get; set; }
        public Guid GoodsId { get; set; }
    }
}
