using System;

namespace RENT.Domain.Dtos
{
    public class AddressDto
    {
        public Guid AddressId { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
        public string? CompanyCode { get; set; }
    }
}
