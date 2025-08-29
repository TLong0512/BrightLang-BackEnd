using Application.Dtos.ExamTypeDtos;
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
    public class ExamTypeService : IExamTypeService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        
        public ExamTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task AddExamTypeAsync(ExamTypeAddDto examTypeAddDto, Guid userId   )
        {
            var newExamType = _mapper.Map<ExamType>(examTypeAddDto);
            await _unitOfWork.ExamTypeRepository.AddAsync(newExamType,userId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteExamTypeAsync(Guid id)
        {
            var examType = await _unitOfWork.ExamTypeRepository.GetByIdAsync(id);

            if (examType == null)
            {
                return false;
            }
            else
            {
                await _unitOfWork.ExamTypeRepository.Delete(examType);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }

        public async Task<IEnumerable<ExamTypeViewDto>> GellAllExamTypeAsync()
        {
            var result = await _unitOfWork.ExamTypeRepository.GetAllAsync();
            var listResultDto = _mapper.Map<IEnumerable<ExamTypeViewDto>>(result);
            return listResultDto;
        }

        public async Task<ExamTypeViewDto> GetExamTypeByIdAsync(Guid id)
        {
            var result = await _unitOfWork.ExamTypeRepository.GetByIdAsync(id);
            return _mapper.Map<ExamTypeViewDto>(result);
        }

        public async Task<ExamTypeViewDto> UpdateExamTypeAsync(Guid id, ExamTypeUpdateDto examTypeUpdateDto, Guid userId)
        {
            var examType = await _unitOfWork.ExamTypeRepository.GetByIdAsync(id);

            if (examType == null)
            {
                return null;
            }
            else
            {
                var updaedExamType = _mapper.Map<ExamType>(examTypeUpdateDto);
                examType.Name = updaedExamType.Name;
                examType.Description = updaedExamType.Description;
                await _unitOfWork.ExamTypeRepository.Update(examType, userId );
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<ExamTypeViewDto>(examType);
            }
        }
    }
}
