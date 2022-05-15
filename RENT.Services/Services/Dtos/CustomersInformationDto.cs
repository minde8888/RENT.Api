using System;
using System.Collections.Generic;
using RENT.Domain.Dtos;

namespace RENT.Services.Services.Dtos
{
    public class CustomersInformationDto : BaseEntityDto
    {
        public Guid CustomersId { get; set; }
        public string? Token { get; set; }
    }
}
