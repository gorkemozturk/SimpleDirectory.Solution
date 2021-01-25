using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDirectory.Extension.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        Task<T[]> GetResourcesAsync();
        Task<T> GetResourceAsync(Guid id);
        T CreateResource(T resource);
        T UpdateResource(T resource);
        T DeleteResource(T resource);
        Task<int> SaveChangesAsync();
    }
}
