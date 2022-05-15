namespace RENT.Domain.Entities
{
    public class Goods
    {
        public Guid GoodsId { get; set; }
        public Guid SellerId { get; set; }
        public Guid CustomersId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Categories>? Categories { get; set; }
    }
}
