
namespace RENT.Domain.Dtos
{
    public class UserRegistrationDto : BaseEntityDto
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
    }
}