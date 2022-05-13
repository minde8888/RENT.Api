using System;

namespace WTP.Domain.Dtos
{
    public class AddressDto
    {
        public string Street { get; set; } = "Kleiva";
        public string City { get; set; } = "Fauske";
        public string Country { get; set; } = "Norway";
        public string Zip { get; set; } = "8200";
        public Guid? ManagerId { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
