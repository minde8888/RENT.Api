using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTP.Data.Context;
using WTP.Domain.Dtos;

namespace WTP.Services.Services
{
    public class ProgressPlanService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ProgressPlanService(AppDbContext context,
            IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public ProgressPlanDto GetOnePlan(ProgressPlanDto progressPlan)
        {
            var planUpdated = _context.ProgressPlan.Find(progressPlan.ProgressPlanId);
            var planToReturn = _mapper.Map<ProgressPlanDto>(planUpdated);
            return planToReturn;
        }
    }
}