using Microsoft.AspNetCore.Http;

namespace RENT.Domain.Dtos.RequestDto
{
    public class RequestUserDto : BaseEntityDto
    {
        public IFormFile ImageFile { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
    }
}