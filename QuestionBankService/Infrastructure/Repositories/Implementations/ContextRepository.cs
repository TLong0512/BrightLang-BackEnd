using Domain.Entities;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    public class ContextRepository : GenericRepository<Context>, IContextRepository
    {
        public ContextRepository(DefaultContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Context>> GetAllContextsAsync()
        {
            return await _context.Contexts.Include(x => x.Range).Include(x => x.Questions).ToListAsync();
        }

        public async Task<IEnumerable<Context>> GetContextByCondition(Expression<Func<Context, bool>> predicate)
        {
            return await _context.Contexts.Include(x => x.Range).Include(x => x.Questions).Where(predicate).ToListAsync();
        }

        public async Task<Context> GetContextById(Guid id)
        {
            return await _context.Contexts.Include(x => x.Range).Include(x => x.Questions).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
