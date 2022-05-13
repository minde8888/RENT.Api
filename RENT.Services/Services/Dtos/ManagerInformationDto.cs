using System;
using System.Collections.Generic;
using WTP.Domain.Dtos;

namespace WTP.Services.Services.Dtos
{
    public class ManagerInformationDto : BaseEntityDto
    {
        public Guid ManagerId { get; set; }
        public ICollection<EmployeeDto> Employees { get; set; }
        public string Token { get; set; }
    }
}
