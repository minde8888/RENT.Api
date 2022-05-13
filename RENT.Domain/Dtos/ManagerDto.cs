using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace WTP.Domain.Dtos
{
    public class ManagerDto : BaseEntityDto
    {
        public ICollection<EmployeeDto> Employees { get; set; }
        public Guid? ProjectId { get; set; }
        public ICollection<ProjectDto> Project { get; set; }
    }
}
