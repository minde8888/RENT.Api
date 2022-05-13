using System;

namespace WTP.Domain.Dtos
{
    public class ReturnEmployeeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Occupation { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageName { get; set; }
        public string ImageSrc { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public AddressDto Address { get; set; }
    }
}