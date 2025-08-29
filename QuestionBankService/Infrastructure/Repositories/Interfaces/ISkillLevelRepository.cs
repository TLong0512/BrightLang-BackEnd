using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface ISkillLevelRepository : IGenericRepository<SkillLevel>
    {
        public Task<IEnumerable<SkillLevel>> GetAllSkillLevelsAsync();
        public Task<SkillLevel> GetSkillLevelById(Guid id);
        public Task<IEnumerable<SkillLevel>> GetSkillLevelByCondition(Expression<Func<SkillLevel, bool>> predicate);

    }
}
