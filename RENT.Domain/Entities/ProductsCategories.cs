﻿

namespace RENT.Domain.Entities
{
    public class ProductsCategories
    {
        public Guid CategoriesId { get; set; }
        public Categories Categories { get; set; }
        public Guid ProductsId { get; set; }
        public Products Products { get; set; }
    }
}
