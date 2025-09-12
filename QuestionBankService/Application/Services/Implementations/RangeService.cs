using Application.Dtos.RangeDtos;
using Application.Dtos.SkillLevelDto;
using Application.Services.Intefaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

        public async Task AddRangeAsync(RangeAddDto rangeAddDto, Guid userId)
        {

            var existingSkillLevel = await _unitOfWork.SkillLevelRepository.GetByIdAsync(rangeAddDto.SkillLevelId);
            if (existingSkillLevel == null)
            {
                throw new KeyNotFoundException("Not found skill level");
            }
            if (rangeAddDto.StartQuestionNumber == 0 || rangeAddDto.EndQuestionNumber == 0)
                throw new ArgumentException("wrong range number");
            if (rangeAddDto.StartQuestionNumber > rangeAddDto.EndQuestionNumber)
                throw new ArgumentException("start number must be bigger than end number");
            var existingRange = await _unitOfWork.RangeRepository.GetByConditionAsync(x => x.Name == rangeAddDto.Name);
            if(existingRange.Count() > 0)
            {
                throw new InvalidOperationException("Range already existed in db");
            }
            var newRange = _mapper.Map<Range>(rangeAddDto);
            await _unitOfWork.RangeRepository.AddAsync(newRange, userId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteRangeAsync(Guid id)
        {
            var range = await _unitOfWork.RangeRepository.GetByIdAsync(id);

            if (range == null)
            {
                throw new KeyNotFoundException("Not found range");
            }

            await _unitOfWork.RangeRepository.Delete(range);
            await _unitOfWork.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<RangeViewDto>> GellAllRangeAsync()
        {
            var result = await _unitOfWork.RangeRepository.GetAllAsync();
            var listResultDto = _mapper.Map<IEnumerable<RangeViewDto>>(result);
            return listResultDto;
        }

        public async Task<IEnumerable<RangeWithQuestionNumberDto>> GetAllRangeIdByExamTypeAndSkill(Guid examTypeId, Guid skillId)
        {
            var levels = await _unitOfWork.LevelRepository
                .GetByConditionAsync(x => x.ExamTypeId == examTypeId);

            if (levels == null || !levels.Any())
                return Enumerable.Empty<RangeWithQuestionNumberDto>();

            var levelIds = levels.Select(x => x.Id);

            var skillLevels = await _unitOfWork.SkillLevelRepository
                .GetByConditionAsync(x => x.SkillId == skillId && levelIds.Contains(x.LevelId));

            if (skillLevels == null || !skillLevels.Any())
                return Enumerable.Empty<RangeWithQuestionNumberDto>();

            var skillLevelIds = skillLevels.Select(x => x.Id);

            var ranges = await _unitOfWork.RangeRepository
                .GetByConditionAsync(x => skillLevelIds.Contains(x.SkillLevelId));

            if (ranges == null || !ranges.Any())
                return Enumerable.Empty<RangeWithQuestionNumberDto>();

            return _mapper.Map<IEnumerable<RangeWithQuestionNumberDto>>(ranges);
        }
        public async Task<RangeViewDto> GetRangeByIdAsync(Guid id)
        {
            var result = await _unitOfWork.RangeRepository.GetByIdAsync(id);

            if (result == null)
            {
                throw new KeyNotFoundException($"No range has id {id}");
            }

            return _mapper.Map<RangeViewDto>(result);
        }

        public async Task<IEnumerable<RangeViewDto>> GetRangesBySkillLevelIdAsync(Guid skillLevelId)
        {
            var existingSkillLevel = await _unitOfWork.SkillLevelRepository.GetSkillLevelById(skillLevelId);
            if (existingSkillLevel == null)
            {
                return Enumerable.Empty<RangeViewDto>();
            }

            var rangesInSkillLevel = await _unitOfWork.RangeRepository.GetByConditionAsync(x => x.SkillLevelId == skillLevelId);
            var orderedSkillLevel = rangesInSkillLevel.OrderBy(x => x.StartQuestionNumber);
            return _mapper.Map<IEnumerable<RangeViewDto>>(orderedSkillLevel);
        }

        public async Task<RangeViewDto> UpdateRangeAsync(Guid id, RangeUpdateDto rangeUpdateDto, Guid userId)
        {
            var range = await _unitOfWork.RangeRepository.GetByIdAsync(id);
            if (range == null)
            {
                throw new KeyNotFoundException("Not found range");
            }

            var existingSkillLevel = await _unitOfWork.SkillLevelRepository.GetByIdAsync(rangeUpdateDto.SkillLevelId);
            if (existingSkillLevel == null)
            {
                throw new KeyNotFoundException("Not found skill level");
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
