using Application.Dtos.RangeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Intefaces
{
    public interface IRangeService
    {
        public Task<IEnumerable<RangeViewDto>> GellAllRangeAsync();
        public Task<RangeViewDto> GetRangeByIdAsync(Guid id);
        public Task<bool> AddRangeAsync(RangeAddDto RangeAddDto);
        public Task<RangeViewDto> UpdateRangeAsync(Guid id, RangeUpdateDto RangeUpdateDto);
        public Task<bool> DeleteRangeAsync(Guid id);
        public Task<IEnumerable<RangeViewDto>>GetRangesBySkillLevelIdAsync(Guid skillLevelId);
    }
}
