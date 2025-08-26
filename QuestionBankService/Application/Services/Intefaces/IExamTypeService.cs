using Application.Dtos.ExamTypeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Intefaces
{
    public interface IExamTypeService
    {
        public Task<IEnumerable<ExamTypeViewDto>> GellAllExamTypeAsync();
        public Task<ExamTypeViewDto> GetExamTypeByIdAsync(Guid id);
        public Task AddExamTypeAsync(ExamTypeAddDto examTypeAddDto);
        public Task<ExamTypeViewDto> UpdateExamTypeAsync(Guid id, ExamTypeUpdateDto examTypeUpdateDto);
        public Task<bool> DeleteExamTypeAsync(Guid id);
    }
}
