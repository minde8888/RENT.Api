using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTP.Domain.Dtos.UpdateDto
{
    public class RequestEmployeeDto
    {
        public Guid Id { get; set; }
        public DateTime? DateUpdated { get; set; } = DateTime.UtcNow;
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Occupation { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageName { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageSrc { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public Guid? ManagerId { get; set; }
        public AddressDto Address { get; set; }
    }
}
