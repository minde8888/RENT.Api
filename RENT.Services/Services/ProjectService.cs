using AutoMapper;
using System;
using WTP.Data.Context;
using WTP.Domain.Dtos;

namespace WTP.Services.Services
{
    public class ProjectService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context,
            IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public ProjectDto GetOneProject(Guid id)
        {
            var projectUpdated = _context.Project.Find(id);
            var projectToReturn = _mapper.Map<ProjectDto>(projectUpdated);
            return projectToReturn;
        }
    }
}