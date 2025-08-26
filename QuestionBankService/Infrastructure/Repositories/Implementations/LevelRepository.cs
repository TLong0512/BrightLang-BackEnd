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
    public class LevelRepository : GenericRepository<Level>, ILevelRepository
    {
        public LevelRepository(DefaultContext context) : base(context)
        {
            
        }
        public async Task<IEnumerable<Level>> GetAllLevelsAsync()
        {
            return await _context.Levels.Include(x => x.ExamType).ToListAsync();
        }

        public async Task<Level> GetLevelById(Guid id)
        {
            return await _context.Levels.Include(x => x.ExamType).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
