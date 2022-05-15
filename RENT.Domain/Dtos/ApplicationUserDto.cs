using RENT.Domain.Entities;
using System;

namespace RENT.Domain.Dtos
{
    public class ApplicationUserDto
    {
        public Seller? Seller { get; set; }
        public Customers? Customers { get; set; }
        public Guid? CustomersId { get; set; }
    }
}
