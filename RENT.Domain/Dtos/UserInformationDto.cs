
namespace RENT.Domain.Dtos
{
    public class UserInformationDto : BaseEntityDto
    {

        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}