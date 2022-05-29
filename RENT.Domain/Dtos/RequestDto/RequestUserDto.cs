using Microsoft.AspNetCore.Http;

namespace RENT.Domain.Dtos.RequestDto
{
    public class RequestUserDto : BaseEntityDto
    {
        public IList<IFormFile> ImageFile { get; set; }
        public  IList<string> Height { get; set; }
        public  IList<string> Width { get; set; }
    }
}