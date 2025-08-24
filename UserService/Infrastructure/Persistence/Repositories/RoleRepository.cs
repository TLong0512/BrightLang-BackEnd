using Application.Abstraction.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RoleRepository : GenericRepository<Role, Guid>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetByRoleNameAsync(string roleName)
    {
        Role? entity = await _context.Roles
            .FirstOrDefaultAsync(r => r.RoleName.ToLower() == roleName.ToLower());
        return entity;
    }
}
