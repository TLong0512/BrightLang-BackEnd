using Application.Abstraction.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        List<T> entities = await _context.Set<T>().ToListAsync();
        return entities;
    }

    public virtual async Task<T?> GetByIdAsync(TKey id)
    {
        T? entity = await _context.Set<T>().FindAsync(id);
        return entity;
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        List<T> matchEntities = await _context.Set<T>().Where(predicate).ToListAsync();
        return matchEntities;
    }




    public virtual async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return;
    }

    public virtual Task UpdateAsync(T entity)
    {
        //_context.Entry(entity).State = EntityState.Modified;
        _context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(TKey id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity == null)
        {
            return;
        }
        _context.Set<T>().Remove(entity);
        return;
    }
}
