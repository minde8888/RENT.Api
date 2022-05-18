
using Microsoft.AspNetCore.Http;

namespace RENT.Domain.Dtos.ResponseDto
{
    public class ResponseUserDto : BaseEntityDto
    {
        public IFormFile ImageFile { get; set; }
    }
}
