using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace NWCodeFirstMVC.Infrastructure.Repositories
{
    public class GenericRepository<T> where T : class
    {
        protected readonly PgNwContext _dc;

        public GenericRepository(PgNwContext dc)
        {
            _dc = dc;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dc.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> GetAsync(int? id)
        {
            if (id == null)
                return null;

            // Resolve primary key CLR type for entity T and convert the incoming id to that type
            var entityType = _dc.Model.FindEntityType(typeof(T));
            if (entityType == null || entityType.FindPrimaryKey() == null)
            {
                // Fallback to FindAsync with the int value
                return await _dc.Set<T>().FindAsync(id.Value);
            }

            var keyProperty = entityType.FindPrimaryKey().Properties.First();
            var keyClrType = keyProperty.ClrType;

            try
            {
                var convertedKey = Convert.ChangeType(id.Value, keyClrType);
                var found = await _dc.Set<T>().FindAsync(new object[] { convertedKey });
                return found;
            }
            catch
            {
                // If conversion fails, try direct FindAsync as a last resort
                return await _dc.Set<T>().FindAsync(id.Value);
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dc.Set<T>().AddAsync(entity);
            await _dc.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            var key = _dc.Model.FindEntityType(typeof(T))!.FindPrimaryKey()!;
            var keyValues = key.Properties
                .Select(p => _dc.Entry(entity).Property(p.Name).CurrentValue)
                .ToArray();

            var tracked = _dc.Set<T>().Local
                .FirstOrDefault(e => key.Properties
                    .Select(p => _dc.Entry(e).Property(p.Name).CurrentValue)
                    .SequenceEqual(keyValues));

            if (tracked != null && tracked != entity)
            {
                _dc.Entry(tracked).State = EntityState.Detached;
            }

            _dc.Set<T>().Update(entity);
            await _dc.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            if (entity != null)
            {
                _dc.Set<T>().Remove(entity);
                await _dc.SaveChangesAsync();
            }
        }
    }
}

