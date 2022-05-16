

namespace RENT.Domain.Entities
{
    public class GoodsCategories
    {
        public Guid CategoriesId { get; set; }
        public Categories Categories { get; set; }
        public Guid GoodsId { get; set; }
        public Goods Goods { get; set; }

    }
}
