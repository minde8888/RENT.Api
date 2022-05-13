using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.Domain.Dtos;
using WTP.Domain.Dtos.UpdateDto;
using WTP.Domain.Entities;

namespace WTP.Data.Interfaces
{
    public interface IEmployeesRepository 
    {
        Task<List<Employee>> GetItemIdAsync(Guid Id);
        public Task AddEmployeeAsync(string userId, RequestEmployeeDto employee);
        public Task UpdateEmployee(RequestEmployeeDto updateManagerDto);
        public Task RemoveEmployeeAsync(string userId);
    }
}
