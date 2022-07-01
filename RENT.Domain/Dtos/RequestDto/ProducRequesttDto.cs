using Microsoft.AspNetCore.Http;

namespace RENT.Domain.Dtos.RequestDto
{
    public class ProducRequesttDto
    {
        public Guid ProductsId { get; set; }
        public IList<IFormFile> Attachments { get; set; }
        public string ImageName { get; set; }
        public string ImageHeight { get; set; }
        public string ImageWidth { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string QuantityPerUnit { get; set; }
        public string UnitPrice { get; set; }
        public string UnitsInStock { get; set; }
        public string WarehousePlace { get; set; }
        public string ProductCode { get; set; }
        public string Categories { get; set; }
        public string MaxLoad { get; set; }
        public string Weight { get; set; }
        public string LiftingHeight { get; set; }
        public string Capacity { get; set; }//
        public string EnergySource { get; set; }
        public string Speed { get; set; }
        public string Length { get; set; }
        public string ProductWidth { get; set; }
        public string ProductHeight { get; set; }
        public Guid SellerId { get; set; }
    }
}
