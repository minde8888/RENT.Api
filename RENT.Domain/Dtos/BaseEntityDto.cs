
namespace RENT.Domain.Dtos
{
    public class BaseEntityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string Role { get; set; }
        public IList<string> ImageName { get; set; }
        public IList<string> ImageSrc { get; set; }
        public AddressDto Address { get; set; }
    }
}
