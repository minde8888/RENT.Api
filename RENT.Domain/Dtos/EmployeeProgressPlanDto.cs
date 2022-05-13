using System;

namespace WTP.Domain.Dtos
{
    public class EmployeeProgressPlanDto
    {
        public Guid EmployeesId { get; set; }
        public EmployeeDto Employees { get; set; }
        public Guid ProgressPlanId { get; set; }
        public ProgressPlanDto ProgressPlans { get; set; }
    }
}
