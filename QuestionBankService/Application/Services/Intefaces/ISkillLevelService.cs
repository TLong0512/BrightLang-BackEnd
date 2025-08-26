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
        public Task<bool> AddSkillLevelAsync(SkillLevelAddDto SkillLevelAddDto);
        public Task<SkillLevelViewDto> UpdateSkillLevelAsync(Guid id, SkillLevelUpdateDto SkillLevelUpdateDto);
        public Task<bool> DeleteSkillLevelAsync(Guid id);
    }
}
