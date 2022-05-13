using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.Domain.Dtos;
using WTP.Domain.Entities;

namespace WTP.Data.Interfaces
{
    public interface IProgressPlanRepository
    {
        public Task<ProgressPlan> AddPlanAsync(ProgressPlanDto progressPlan);
        public Task<List<ProgressPlan>> GetProgressPlanAsync(Guid Id);
        public Task<List<ProgressPlanDto>> GetAllProgressPlansAsync();
        public Task RemoveProgressPlanAsync(Guid id);
        public Task<ProgressPlanReturnDto> UpdateProgressPlanAsync(ProgressPlanDto progressPlanId);
    }
}