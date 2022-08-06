using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RENT.Domain.Entities
{
    public class CategoriesProduct
    {
        public Guid CategoriesId { get; set; }
        public Categories Categories { get; set; }
        public Guid ProductsId { get; set; }
        public Products Products { get; set; }
    }
}
