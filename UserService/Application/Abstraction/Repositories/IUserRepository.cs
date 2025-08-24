using Domain.Entities;

namespace Application.Abstraction.Repositories;

public interface IUserRepository : IGenericRepository<User, Guid>
{
    //Task<IEnumerable<User>> GetByNameAsync(string partOfName);
    Task<User?> GetByEmailAsync(string email);
}

