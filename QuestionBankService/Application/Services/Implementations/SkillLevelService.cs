using Application.Dtos.SkillLevelDto;
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
    public class SkillLevelService : ISkillLevelService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public SkillLevelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task AddSkillLevelAsync(SkillLevelAddDto skillLevelAddDto, Guid userId)
        {

            var existingLevel = await _unitOfWork.LevelRepository.GetByIdAsync(skillLevelAddDto.LevelId);
            var existingSkill = await _unitOfWork.SkillRepository.GetByIdAsync(skillLevelAddDto.SkillId);

            if (existingLevel == null || existingSkill == null)
            {
                throw new ArgumentException("Not found skill or level");
            }
            else
            {
                var newSkillLevel = _mapper.Map<SkillLevel>(skillLevelAddDto);
                await _unitOfWork.SkillLevelRepository.AddAsync(newSkillLevel, userId);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteSkillLevelAsync(Guid id)
        {
            var SkillLevel = await _unitOfWork.SkillLevelRepository.GetByIdAsync(id);

            if (SkillLevel == null)
            {
                return false;
            }
            else
            {
                await _unitOfWork.SkillLevelRepository.Delete(SkillLevel);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }

        public async Task<IEnumerable<SkillLevelViewDto>> GellAllSkillLevelAsync()
        {
            var result = await _unitOfWork.SkillLevelRepository.GetAllSkillLevelsAsync();
            var listResultDto = _mapper.Map<IEnumerable<SkillLevelViewDto>>(result);
            var orderListResultDto = listResultDto.OrderBy(x => x.LevelName);
            return orderListResultDto;
        }

        public async Task<IEnumerable<SkillLevelViewDto>> FilterByLevelIdAsync(Guid LevelId)
        {
            var result = await _unitOfWork.SkillLevelRepository.GetSkillLevelByCondition(x => x.LevelId == LevelId);
            var listResultDto = _mapper.Map<IEnumerable<SkillLevelViewDto>>(result);
            return listResultDto;
        }

        public async Task<SkillLevelViewDto> GetSkillLevelByIdAsync(Guid id)
        {
            var result = await _unitOfWork.SkillLevelRepository.GetSkillLevelById(id);
            return _mapper.Map<SkillLevelViewDto>(result);
        }

        public async Task<SkillLevelViewDto> UpdateSkillLevelAsync(Guid id, SkillLevelUpdateDto skillLevelUpdateDto, Guid userId)
        {
            var skillLevel = await _unitOfWork.SkillLevelRepository.GetSkillLevelById(id);
            if (skillLevel == null)
            {
                return null;
            }
            else
            {
                var existingLevel = await _unitOfWork.LevelRepository.GetByIdAsync(skillLevelUpdateDto.LevelId);
                var existingSkill = await _unitOfWork.SkillRepository.GetByIdAsync(skillLevelUpdateDto.SkillId);

                if (existingLevel == null || existingSkill == null)
                {
                    return null;
                }
                else
                {
                    var updatedSkillLevel = _mapper.Map<SkillLevel>(skillLevelUpdateDto);
                    skillLevel.SkillId = updatedSkillLevel.SkillId;
                    skillLevel.LevelId = updatedSkillLevel.LevelId;
                    await _unitOfWork.SkillLevelRepository.Update(skillLevel,userId);
                    await _unitOfWork.SaveChangesAsync();
                    return _mapper.Map<SkillLevelViewDto>(skillLevel);
                }
            }
        }

    }
}
