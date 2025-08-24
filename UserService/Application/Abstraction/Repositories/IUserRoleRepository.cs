using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Abstraction.Repositories;

public interface IUserRoleRepository
{
    Task<UserRole?> GetByIdAsync(Guid userId, Guid roleId);
    Task<IEnumerable<UserRole>> GetAllAsync();
    Task<IEnumerable<UserRole>> FindAsync(Expression<Func<UserRole, bool>> predicate);


    Task AddAsync(UserRole entity);
    Task UpdateAsync(UserRole entity);
    Task DeleteAsync(Guid userId, Guid roleId);
}
