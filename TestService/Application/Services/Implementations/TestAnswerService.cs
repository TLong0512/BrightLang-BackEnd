using Application.Services.Interfaces;
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
    public class TestAnswerService : ITestAnswerSevice
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        public TestAnswerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddTestAnswerAsync(Guid testId, IEnumerable<Guid> testAnswers, Guid userId)
        {
            await _unitOfWork.TestAnswerRepository.DeleteByCondition(x => x.TestId == testId);
            await _unitOfWork.SaveChangesAsync();

            foreach (var item in testAnswers)
            {
                await _unitOfWork.TestAnswerRepository.AddAnswerAsync(new TestAnswer { AnswerId = item, TestId = testId }, userId);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Guid>> GetAnswerIdsInTestIdAsync(Guid testId)
        {
            var testQuestions = await _unitOfWork.TestAnswerRepository.GetByConditionAsync(x => x.TestId == testId);
            var questionIds = testQuestions.Select(x => x.AnswerId).ToList();
            return questionIds;
        }
    }
}
