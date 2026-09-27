using Microsoft.EntityFrameworkCore;
using NWCodeFirstMVC.Domain.Contracts;
using System.Reflection;
using NWCodeFirstMVC.Infrastructure;

namespace NWCodeFirstMVC.Infrastructure.Services
{
    public class GenericService<T> : IGenericRepository<T> where T : class
    {
        private readonly PgNwContext _dc;

        public GenericService(PgNwContext dc)
        {
            this._dc = dc;
        }

       

        public async Task<T> AddAsync(T entity)
        {
            await _dc.AddAsync(entity);

            try
            {
                await _dc.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if (entity != null)
            {
                TrySetProperty(entity, "IsDeleted", true);
            }

            _dc.SaveChanges();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dc.Set<T>().ToListAsync(); // go to the database and get the dbset that we are requesting, parse them out to list
        }

        public async Task<T?> GetAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            // Force a 500 for testing
            if (id == 999)
                throw new Exception("Forced test exception for 500 response");

            return await _dc.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dc.Update(entity);
            await _dc.SaveChangesAsync();
        }

        private bool TrySetProperty(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(obj, value, null);
                return true;
            }
            return false;
        }
    }
}
