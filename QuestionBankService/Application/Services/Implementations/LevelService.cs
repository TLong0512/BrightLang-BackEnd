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

        public async Task<bool> AddLevelAsync(LevelAddDto levelAddDto)
        {
           
            var existingExamtype = await _unitOfWork.ExamTypeRepository.GetByIdAsync(levelAddDto.ExamTypeId);
            if(existingExamtype == null)
            {
                return false;
            }
            else
            {
                var newLevel = _mapper.Map<Level>(levelAddDto);
                await _unitOfWork.LevelRepository.AddAsync(newLevel, new Guid());
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
            return listResultDto;
        }

        public async Task<LevelViewDto> GetLevelByIdAsync(Guid id)
        {
            var result = await _unitOfWork.LevelRepository.GetLevelById(id);
            return _mapper.Map<LevelViewDto>(result);
        }

        public async Task<LevelViewDto> UpdateLevelAsync(Guid id, LevelUpdateDto levelUpdateDto)
        {
            var level = await _unitOfWork.LevelRepository.GetLevelById(id);
            if (level == null)
            {
                return null;
            }
            else
            {
                
                var existingExamtype = await _unitOfWork.ExamTypeRepository.GetByIdAsync(levelUpdateDto.ExamTypeId);
                if (existingExamtype == null)
                {
                    return null;
                }
                else
                {
                    var updatedLevel = _mapper.Map<Level>(levelUpdateDto);
                    level.ExamTypeId = updatedLevel.ExamTypeId;
                    level.Name = updatedLevel.Name;
                    await _unitOfWork.LevelRepository.Update(level, new Guid());
                    await _unitOfWork.SaveChangesAsync();
                    return _mapper.Map<LevelViewDto>(level);
                }
            }
        }
    }
}
