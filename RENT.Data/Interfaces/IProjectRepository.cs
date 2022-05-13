using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.Domain.Dtos;
using WTP.Domain.Entities;

namespace WTP.Data.Interfaces
{
    public interface IProjectRepository
    {
        public Task<Guid> AddProject(ProjectDto project);
        public Task<List<Project>> GetProjectAsync(Guid Id);
        public Task<List<ProjectDto>> GetAllProjects();
        public Task UpdateProjectAsync(ProjectDto project);
        public Task RemoveProjectAsync(Guid id);
    }
}
