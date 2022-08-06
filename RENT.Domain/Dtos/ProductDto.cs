using Microsoft.AspNetCore.Http;

namespace RENT.Domain.Dtos.RequestDto
{
    public class ProductDto
    {
        public Guid ProductsId { get; set; }
        public IList<IFormFile> Images { get; set; }
        public IList<string> ImageName { get; set; }
        public IList<string> ImageSrc { get; set; }
        public IList<string> Height { get; set; }
        public IList<string> Width { get; set; }
        public string ProductName { get; set; }
        public string QuantityPerUnit { get; set; }
        public string UnitPrice { get; set; }
        public string UnitsInStock { get; set; }
        public string WarehousePlace { get; set; }
        public string ProductCode { get; set; }
        public Guid SellerId { get; set; }
        public IList<string> Category { get; set; }
        public PostsDto Posts { get; set; }
        public ProductsSpecificationsDto Specifications { get; set; }
    }
}
