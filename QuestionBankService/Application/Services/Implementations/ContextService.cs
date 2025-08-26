using Application.Dtos.ContextDtos;
using Application.Services.Intefaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class ContextService : IContextService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public ContextService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<Guid> AddContextAsync(ContextAddDto contextAddDto)
        {

            var existingRange = await _unitOfWork.RangeRepository.GetByIdAsync(contextAddDto.RangeId);
            if (existingRange == null)
            {
                return Guid.Empty;
            }
            else
            {
                var newContext = _mapper.Map<Context>(contextAddDto);
                await _unitOfWork.ContextRepository.AddAsync(newContext, new Guid());
                await _unitOfWork.SaveChangesAsync();
                return newContext.Id;
            }
        }

        public async Task<bool> DeleteContextAsync(Guid id)
        {
            var context = await _unitOfWork.ContextRepository.GetByIdAsync(id);

            if (context == null)
            {
                return false;
            }
            else
            {
                await _unitOfWork.ContextRepository.Delete(context);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }

        public async Task<IEnumerable<ContextViewDto>> GellAllContextAsync()
        {
            var result = await _unitOfWork.ContextRepository.GetAllContextsAsync();
            var listResultDto = _mapper.Map<IEnumerable<ContextViewDto>>(result);
            return listResultDto;
        }

        public async Task<ContextViewDto> GetContextByIdAsync(Guid id)
        {
            var result = await _unitOfWork.ContextRepository.GetContextById(id);
            return _mapper.Map<ContextViewDto>(result);
        }

        public async Task<ContextViewDto> UpdateContextAsync(Guid id, ContextUpdateDto contextUpdateDto)
        {
            var context = await _unitOfWork.ContextRepository.GetContextById(id);
            if (context == null)
            {
                return null;
            }
            else
            {

                var existingRange = await _unitOfWork.RangeRepository.GetByIdAsync(contextUpdateDto.RangeId);
                if (existingRange == null)
                {
                    return null;
                }
                else
                {
                    var updatedContext = _mapper.Map<Context>(contextUpdateDto);
                    context.RangeId = updatedContext.RangeId;
                    context.Content = updatedContext.Content;
                    context.Explain = updatedContext.Explain;
                    context.IsBelongTest = updatedContext.IsBelongTest;
                    await _unitOfWork.ContextRepository.Update(context, new Guid());
                    await _unitOfWork.SaveChangesAsync();
                    return _mapper.Map<ContextViewDto>(context);
                }
            }
        }
    }
}
