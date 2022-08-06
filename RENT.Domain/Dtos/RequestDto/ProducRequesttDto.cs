using Microsoft.AspNetCore.Http;

namespace RENT.Domain.Dtos.RequestDto
{
    public class ProducRequesttDto
    {

        public List<IFormFile> Images { get; set; }
        public string ImageHeight { get; set; }
        public string ImageWidth { get; set; }
        public string ImageName { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string Price { get; set; }
        public string Size { get; set; }
        public string Place { get; set; }
        public string ProductCode { get; set; }
        public string Category { get; set; }
        public Guid SellerId { get; set; }
    }
}