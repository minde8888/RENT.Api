using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RENT.Data.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task AddItemAsync(T t);
        Task DeleteItem(Guid Id);
        Task<IEnumerable<T>> Search(string name);
    }
}
