using Application.Abstraction.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    protected readonly AppDbContext _context;
    public UserRoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public virtual async Task<IEnumerable<UserRole>> GetAllAsync()
    {
        List<UserRole> entities = await _context.Set<UserRole>().ToListAsync();
        return entities;
    }

    public virtual async Task<UserRole?> GetByIdAsync(Guid userId, Guid roleId)
    {
        UserRole? entity = await _context.Set<UserRole>().FindAsync(userId, roleId);
        return entity;
    }

    public virtual async Task<IEnumerable<UserRole>> FindAsync(Expression<Func<UserRole, bool>> predicate)
    {
        List<UserRole> matchEntities = await _context.Set<UserRole>().Where(predicate).ToListAsync();
        return matchEntities;
    }




    public virtual async Task AddAsync(UserRole entity)
    {
        await _context.Set<UserRole>().AddAsync(entity);
        return;
    }

    public virtual Task UpdateAsync(UserRole entity)
    {
        //_context.Entry(entity).State = EntityState.Modified;
        _context.Set<UserRole>().Update(entity);
        return Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(Guid userId, Guid roleId)
    {
        var entity = await _context.Set<UserRole>().FindAsync(userId, roleId);
        if (entity == null)
        {
            return;
        }
        _context.Set<UserRole>().Remove(entity);
        return;
    }
}
