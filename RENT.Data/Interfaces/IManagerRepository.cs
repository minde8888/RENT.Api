using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.Domain.Dtos;
using WTP.Domain.Dtos.UpdateDto;
using WTP.Domain.Entities;

namespace WTP.Data.Interfaces
{
    public interface IManagerRepository
    {
        public Task<List<Manager>> GetItemIdAsync(Guid Id);

        public Task<List<ManagerDto>> GetManagerAsync(string ImageSrc);

        public Task AddManagerAsync(Manager manager, string UserId);

        public Task<Manager> UpdateManager(RequestManagerDto updateManagerDto);

        public Task RemoveManagerAsync(string userId);
    }
}