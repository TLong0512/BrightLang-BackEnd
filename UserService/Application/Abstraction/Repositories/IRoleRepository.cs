using Domain.Entities;

namespace Application.Abstraction.Repositories;

// generic repository doesn't fit here
// because role is a immutable table
public interface IRoleRepository : IGenericRepository<Role, Guid>
{
    Task<Role?> GetByRoleNameAsync(string roleName);
}
