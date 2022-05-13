using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.Domain.Entities;

namespace WTP.Domain.Dtos
{
    public class BaseEntityDto
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Occupation { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ImageName { get; set; }
        public string ImageSrc { get; set; }
        public bool IsDeleted { get; set; } = false;
        public AddressDto Address { get; set; }
        public ICollection<PostDto> Posts { get; set; }
    }
}
