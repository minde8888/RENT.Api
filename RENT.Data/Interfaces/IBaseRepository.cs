using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WTP.Data.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task AddItemAsync(T t);
        Task DeleteItem(Guid Id);
        Task<IEnumerable<T>> Search(string name);
    }
}
