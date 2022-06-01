using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RENT.Domain.Dtos
{
    public class Temp
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string Role { get; set; }
        public string ImageName { get; set; }
        public AddressDto AddressDto { get; set; }
    }
}
