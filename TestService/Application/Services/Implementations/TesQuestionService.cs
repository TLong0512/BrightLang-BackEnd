using Application.Services.Interfaces;
using AutoMapper;
using Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class TesQuestionService : ITestQuestionService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        public TesQuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Guid>> GetQuestionIdsInTestIdAsync(Guid testId)
        {
            var testQuestions = await _unitOfWork.TestQuestionRepository.GetByConditionAsync(x => x.TestId == testId);
            var questionIds = testQuestions.Select(x => x.QuestionId).ToList();
            return questionIds;
        }
    }
}
