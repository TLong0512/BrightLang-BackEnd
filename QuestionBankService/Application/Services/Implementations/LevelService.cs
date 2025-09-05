using Application.Dtos.ExamTypeDtos;
using Application.Dtos.LevelDtos;
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
    public class LevelService : ILevelService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public LevelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<bool> AddLevelAsync(LevelAddDto levelAddDto, Guid userId)
        {

            var existingLevel = await _unitOfWork.ExamTypeRepository.GetByIdAsync(levelAddDto.ExamTypeId);
            if (existingLevel == null)
            {
                return false;
            }
            else
            {
                var existingExamType = _unitOfWork.LevelRepository.GetByConditionAsync(x => x.Name == levelAddDto.Name);
                if (existingExamType == null)
                {
                    throw new InvalidOperationException("Level has already in db");
                }
                var newLevel = _mapper.Map<Level>(levelAddDto);
                await _unitOfWork.LevelRepository.AddAsync(newLevel, userId);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteLevelAsync(Guid id)
        {
            var level = await _unitOfWork.LevelRepository.GetByIdAsync(id);

            if (level == null)
            {
                return false;
            }
            else
            {
                await _unitOfWork.LevelRepository.Delete(level);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }

        public async Task<IEnumerable<LevelViewDto>> GellAllLevelAsync()
        {
            var result = await _unitOfWork.LevelRepository.GetAllLevelsAsync();
            var listResultDto = _mapper.Map<IEnumerable<LevelViewDto>>(result);
            var orderedListResultDto = listResultDto.OrderBy(x => x.Name);
            return orderedListResultDto;
        }

        public async Task<IEnumerable<LevelViewDto>> GetLevelByExamTypeId(Guid examTypeId)
        {
            var levels = await _unitOfWork.LevelRepository.GetByConditionAsync(x => x.ExamTypeId == examTypeId);

            if (levels == null || !levels.Any())
            {
                return Enumerable.Empty<LevelViewDto>();
            }
            return _mapper.Map<IEnumerable<LevelViewDto>>(levels);
        }

        public async Task<LevelViewDto> GetLevelByIdAsync(Guid id)
        {
            var result = await _unitOfWork.LevelRepository.GetLevelByIdAsync(id);
            return _mapper.Map<LevelViewDto>(result);
        }

        public async Task<LevelViewDto> UpdateLevelAsync(Guid id, LevelUpdateDto levelUpdateDto, Guid userId)
        {
            var level = await _unitOfWork.LevelRepository.GetLevelByIdAsync(id);
            if (level == null)
            {
                throw new KeyNotFoundException("Not found level");
            }
            var updatedLevel = _mapper.Map<Level>(levelUpdateDto);
            level.ExamTypeId = updatedLevel.ExamTypeId;
            level.Name = updatedLevel.Name;
            await _unitOfWork.LevelRepository.Update(level, userId);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<LevelViewDto>(level);

        }
    }
}
