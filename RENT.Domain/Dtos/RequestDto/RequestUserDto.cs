using Microsoft.AspNetCore.Http;

namespace RENT.Domain.Dtos.RequestDto
{
    public class RequestUserDto 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string Roles { get; set; }
        public string ImageName { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string CompanyCode { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}