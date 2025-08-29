using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DefaultContext _context;
        public GenericRepository(DefaultContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity, Guid userId)
        {
            var createdDateProp = entity.GetType().GetProperty("CreatedDate");
            var createdByProp = entity.GetType().GetProperty("CreatedBy");

            if (createdDateProp != null)
                createdDateProp.SetValue(entity, DateTime.Now);

            if (createdByProp != null)
                createdByProp.SetValue(entity, userId);

            await _context.Set<T>().AddAsync(entity);
        }

        public Task Delete(T entity)
        {
            entity.GetType().GetProperty("IsDeleted")?.SetValue(entity, true);
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public Task Update(T entity, Guid userId)
        {
            var createdDateProp = entity.GetType().GetProperty("UpdatedDate");
            var createdByProp = entity.GetType().GetProperty("UpdatedBy");

            if (createdDateProp != null)
                createdDateProp.SetValue(entity, DateTime.Now);

            if (createdByProp != null)
                createdByProp.SetValue(entity, userId);

            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
