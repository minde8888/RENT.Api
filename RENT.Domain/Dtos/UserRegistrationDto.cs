
namespace RENT.Domain.Dtos
{
    public class UserRegistrationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }
    }
}