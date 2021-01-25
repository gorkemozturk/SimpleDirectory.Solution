using Microsoft.EntityFrameworkCore;
using SimpleDirectory.Data;
using SimpleDirectory.Extension.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDirectory.Extension.Services
{
    public abstract class BaseService<T> : IBaseService<T> where T : class
    {
        protected readonly DirectoryDbContext _context;

        public BaseService(DirectoryDbContext context)
        {
            _context = context;
        }

        public async Task<T[]> GetResourcesAsync()
        {
            return await _context.Set<T>().ToArrayAsync();
        }

        public async Task<T> GetResourceAsync(Guid id)
        {
            var resource = await _context.Set<T>().FindAsync(id);

            if (resource == null)
            {
                return null;
            }

            return resource;
        }

        public T CreateResource(T resource)
        {
            _context.Set<T>().Add(resource); 
            return resource;
        }

        public T UpdateResource(T resource)
        {
            _context.Entry(resource).State = EntityState.Modified;
            return resource;
        }

        public T DeleteResource(T resource)
        {
            _context.Set<T>().Remove(resource); 
            return resource;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
