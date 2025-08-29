using Application.Dtos.LevelDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Intefaces
{
    public interface ILevelService
    {
        public Task<IEnumerable<LevelViewDto>> GellAllLevelAsync();
        public Task<LevelViewDto> GetLevelByIdAsync(Guid id);
        public Task<bool> AddLevelAsync(LevelAddDto LevelAddDto, Guid userId);
        public Task<LevelViewDto> UpdateLevelAsync(Guid id, LevelUpdateDto LevelUpdateDto, Guid userId);
        public Task<bool> DeleteLevelAsync(Guid id);
    }
}
