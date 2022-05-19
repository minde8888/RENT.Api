using Microsoft.AspNetCore.Http;

namespace RENT.Domain.Dtos.RequestDto
{
    public class ProductDto
    {
        public Guid ProductsId { get; set; }
        public IList<IFormFile> Attachments { get; set; }
        public IList<string> ImageName { get; set; }
        public IList<string> ImageSrc { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string ProductName { get; set; }
        public int QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public string WarehousePlace { get; set; }
        public Guid SellerId { get; set; }
        public IList<string> Category { get; set; }
        public PostsDto Posts { get; set; }
        public ProductsSpecificationsDto Specifications { get; set; }
    }
}
