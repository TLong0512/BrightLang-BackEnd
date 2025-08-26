using Domain.Entities;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return await _context.Contexts.Include(x => x.Range).ToListAsync();
        }

        public async Task<Context> GetContextById(Guid id)
        {
            return await _context.Contexts.Include(x => x.Range).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
