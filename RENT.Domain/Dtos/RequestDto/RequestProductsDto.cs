using Microsoft.AspNetCore.Http;

namespace RENT.Domain.Dtos.RequestDto
{
    public class RequestProductsDto
    {
        public Guid ProductsId { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string ProductName { get; set; }
        public int QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public string WarehousePlace { get; set; }
        public Guid SellerId { get; set; }
        public string CategoryIds { get; set; }
    }
}
