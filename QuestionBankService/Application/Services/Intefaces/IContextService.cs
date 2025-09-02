using Application.Dtos.ContextDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Intefaces
{
    public interface IContextService
    {
        public Task<IEnumerable<ContextViewDto>> GellAllContextAsync();
        public Task<ContextViewDto> GetContextByIdAsync(Guid id);
        public Task<ContextViewDto> GetContextByRangeIdAsync(Guid rangeId);
        public Task<Guid> AddContextAsync(ContextAddDto ContextAddDto, Guid userId);
        public Task<ContextViewDto> UpdateContextAsync(Guid id, ContextUpdateDto ContextUpdateDto, Guid userId);
        public Task<bool> DeleteContextAsync(Guid id);
    }
}
