using Application.Dtos.BaseDtos;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Abstraction.Repositories;

public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<PageResultDto<TEntity>> GetAllAsync(int page = 1, int pageSize = 10);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);


    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TKey id);
}


