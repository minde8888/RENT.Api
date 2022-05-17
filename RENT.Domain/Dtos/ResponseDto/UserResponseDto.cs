
using Microsoft.AspNetCore.Http;

namespace RENT.Domain.Dtos.ResponseDto
{
    public class UserResponseDto : BaseEntityDto
    {
        public IFormFile ImageFile { get; set; }
    }
}
