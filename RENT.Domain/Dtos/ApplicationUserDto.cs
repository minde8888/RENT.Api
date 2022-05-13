using System;

namespace WTP.Domain.Dtos
{
    public class ApplicationUserDto
    {
        public EmployeeDto Employees { get; set; }
        public ManagerDto Manager { get; set; }
        public Guid? ManagerId { get; set; }
    }
}
