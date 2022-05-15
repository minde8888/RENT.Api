using RENT.Domain.Entities;

namespace RENT.Domain.Entities
{
    public class Shop : BaseEntity
    {
        public Guid ShopId { get; set; }
        public Guid AddressId { get; set; }
        public string ShopName { get; set; }

    }
}
