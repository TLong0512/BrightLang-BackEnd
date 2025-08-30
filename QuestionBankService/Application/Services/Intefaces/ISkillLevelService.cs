using Application.Dtos.SkillLevelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Intefaces
{
    public interface ISkillLevelService
    {
        public Task<IEnumerable<SkillLevelViewDto>> GellAllSkillLevelAsync();
        public Task<SkillLevelViewDto> GetSkillLevelByIdAsync(Guid id);
        public Task<bool> AddSkillLevelAsync(SkillLevelAddDto SkillLevelAddDto, Guid userId);
        public Task<SkillLevelViewDto> UpdateSkillLevelAsync(Guid id, SkillLevelUpdateDto SkillLevelUpdateDto, Guid userId);
        public Task<bool> DeleteSkillLevelAsync(Guid id);
        public Task<IEnumerable<SkillLevelViewDto>> FilterByLevelIdAsync(Guid LevelId);
    }
}
