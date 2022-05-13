using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTP.Domain.Entities;

namespace WTP.Data.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<Manager>> GetManagerAsync(string ImageSrc);
        //Task<List<Employee>> GetEmployeeAsync(string ImageSrc);
        public Task AddItemAsync(Manager manager);
    }
}

