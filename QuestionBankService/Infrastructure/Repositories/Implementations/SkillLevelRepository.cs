using Domain.Entities;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    public class SkillLevelRepository : GenericRepository<SkillLevel>, ISkillLevelRepository
    {
        public SkillLevelRepository(DefaultContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SkillLevel>> GetAllSkillLevelsAsync()
        {
            return await _context.SkillLevels.Include(x => x.Skill).Include(y => y.Level).ToListAsync();
        }

        public async Task<SkillLevel> GetSkillLevelById(Guid id)
        {
            return await _context.SkillLevels.Include(x => x.Skill).Include(y => y.Level).FirstOrDefaultAsync(z => z.Id == id);
        }
    }
}
