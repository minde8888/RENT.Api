using Microsoft.AspNetCore.Http;
using System;

namespace WTP.Domain.Dtos.UpdateDto
{
    public class RequestManagerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Occupation { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageName { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageSrc { get; set; }
        public string Role { get; set; }
        public string  Email { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public AddressDto Address { get; set; }
    }
}