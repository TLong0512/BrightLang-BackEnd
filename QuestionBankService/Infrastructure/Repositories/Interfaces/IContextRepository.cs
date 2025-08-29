using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IContextRepository : IGenericRepository<Context>
    {
        public Task<IEnumerable<Context>> GetAllContextsAsync();
        public Task<Context> GetContextById(Guid id);
        public Task<IEnumerable<Context>> GetContextByCondition(Expression<Func<Context, bool>> predicate);
    }
}
