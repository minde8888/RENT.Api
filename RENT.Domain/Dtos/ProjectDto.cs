using System;
using System.Collections.Generic;

namespace WTP.Domain.Dtos
{
    public class ProjectDto
    {
        public Guid ProjectId { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }    
        public string Color { get; set; }
        public Guid? ManagerId { get; set; }
        public ICollection<ProgressPlanReturnDto> ProgressPlan { get; set; }
    }
}
