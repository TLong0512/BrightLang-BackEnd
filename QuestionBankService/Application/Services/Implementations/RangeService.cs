using Application.Dtos.RangeDtos;
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
using Range = Domain.Entities.Range;

namespace Application.Services.Implementations
{
    public class RangeService : IRangeService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public RangeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddRangeAsync(RangeAddDto rangeAddDto, Guid userId)
        {
            
            var existingSkillLevel = await _unitOfWork.SkillLevelRepository.GetByIdAsync(rangeAddDto.SkillLevelId);
            if (existingSkillLevel == null)
            {
                return false;
            }
            else
            {
                var newRange = _mapper.Map<Range>(rangeAddDto);
                await _unitOfWork.RangeRepository.AddAsync(newRange,userId);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteRangeAsync(Guid id)
        {
            var range = await _unitOfWork.RangeRepository.GetByIdAsync(id);

            if (range == null)
            {
                return false;
            }
            else
            {
                await _unitOfWork.RangeRepository.Delete(range);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }

        public async Task<IEnumerable<RangeViewDto>> GellAllRangeAsync()
        {
            var result = await _unitOfWork.RangeRepository.GetAllAsync();
            var listResultDto = _mapper.Map<IEnumerable<RangeViewDto>>(result);            
            return listResultDto;
        }

        public async Task<IEnumerable<RangeWithQuestionNumberDto>> GetAllRangeIdByExamTypeAndSkill(Guid ExamTypeId, Guid SkillId)
        {
            var listLevel = await _unitOfWork.LevelRepository.GetByConditionAsync(x => x.ExamTypeId == ExamTypeId);
            var listLevelIds = listLevel.Select(x => x.Id);
            var listExistingSkillLevels = await _unitOfWork.SkillLevelRepository.GetByConditionAsync(x => x.SkillId == SkillId &&
                                                                        listLevelIds.Contains(x.LevelId));
            var listExistingSKillLevelIds = listExistingSkillLevels.Select(x => x.Id);
            var listExistingRanges = await _unitOfWork.RangeRepository.GetByConditionAsync(x => listExistingSKillLevelIds.Contains(x.SkillLevelId));
            return _mapper.Map<IEnumerable<RangeWithQuestionNumberDto>>(listExistingRanges);
        }

        public async Task<RangeViewDto> GetRangeByIdAsync(Guid id)
        {
            var result = await _unitOfWork.RangeRepository.GetByIdAsync(id);
            return _mapper.Map<RangeViewDto>(result);
        }

        public async Task<IEnumerable<RangeViewDto>> GetRangesBySkillLevelIdAsync(Guid skillLevelId)
        {
            var existingSkillLevel = await _unitOfWork.SkillLevelRepository.GetSkillLevelById(skillLevelId);
            if(existingSkillLevel == null)
            {
                return null;
            }
            else
            {
                var rangesInSkillLevel = await _unitOfWork.RangeRepository.GetByConditionAsync(x => x.SkillLevelId == skillLevelId);
                var orderedSkillLevel = rangesInSkillLevel.OrderBy(x => x.StartQuestionNumber);
                return _mapper.Map<IEnumerable<RangeViewDto>>(orderedSkillLevel);
            }
        }

        public async Task<RangeViewDto> UpdateRangeAsync(Guid id, RangeUpdateDto rangeUpdateDto, Guid userId)
        {
            var range = await _unitOfWork.RangeRepository.GetByIdAsync(id);
            if (range == null)
            {
                return null;
            }
            else
            {
                var existingSkillLevel = await _unitOfWork.SkillLevelRepository.GetByIdAsync(rangeUpdateDto.SkillLevelId);
                if (existingSkillLevel == null)
                {
                    return null;
                }
                else
                {
                    var updatedRange = _mapper.Map<Range>(rangeUpdateDto);
                    range.Name = updatedRange.Name;
                    range.SkillLevelId = updatedRange.SkillLevelId;
                    range.StartQuestionNumber = updatedRange.StartQuestionNumber;
                    range.EndQuestionNumber = updatedRange.EndQuestionNumber;
                    await _unitOfWork.RangeRepository.Update(range, userId);
                    await _unitOfWork.SaveChangesAsync();
                    return _mapper.Map<RangeViewDto>(range);
                }
            }
        }
    }
}
