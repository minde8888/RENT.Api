using System;

namespace WTP.Domain.Dtos
{
    public class EmployeeDto: BaseEntityDto
    {
        public Guid? ManagerId { get; set; }
        public Guid? ProjectId { get; set; }
    }
}
