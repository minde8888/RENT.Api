
using RENT.Domain.Entities;

namespace RENT.Domain.Dtos.ResponseDto
{
    public class ResponseProductsDto
    {
        public Guid ProductsId { get; set; }
        public string ImageSrc { get; set; }
        public string ProductName { get; set; }
        public int QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public IList<string> ImageName { get; set; }
        public string ProductNumber { get; set; }
        public Seller Seller { get; set; }
        public Customers Customers { get; set; }
        public Posts Posts { get; set; }
        public ProductsSpecifications Specifications { get; set; }
    }
}
