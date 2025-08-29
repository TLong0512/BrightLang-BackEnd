using Application.Dtos.AnswerDtos;
using Application.Dtos.BaseDtos;
using Application.Dtos.QuestionDtos;
using Application.Dtos.TestDtos;
using Application.Services.Interfaces;
using AutoMapper;
using Azure;
using Domain.Entities;
using Infrastructure.Repositories.Intefaces;
using Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Services.Implementations
{
    public class TestService : ITestService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        
        public TestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<Guid> CreateTestAsync(Guid userId, List<Guid> questionIds)
        {
            var testAddDto = new TestAddDto {UserId = userId };
            var test = _mapper.Map<Test>(testAddDto);
            await _unitOfWork.TestRepository.AddAsync(test, userId);
            await _unitOfWork.SaveChangesAsync();

            foreach(var questionId in questionIds)
            {
                var testQuestionAddDto = new TestQuestionAddDto { QuestionId = questionId, TestId = test.Id };
                var testQuestion = _mapper.Map<TestQuestion>(testQuestionAddDto);
                await _unitOfWork.TestQuestionRepository.AddAsync(testQuestion, userId);
                await _unitOfWork.SaveChangesAsync();
            }

            return test.Id;
        }
        
        public async Task<PageResult<TestSummaryDto>> GetAllTestByUserIdAsync(Guid userId, int page = 1, int pageSize = 10)
        {

            var query = _unitOfWork.TestRepository
                        .GetAllTestByConditionForPaging(x => x.UserId == userId);

            var totalItems = await query.CountAsync();

            var listTests = await query.Skip((page - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();

            var listTestSummaryDtos =  _mapper.Map<List<TestSummaryDto>>(listTests);

            return new PageResult<TestSummaryDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = listTestSummaryDtos
            };
        }

        public async Task SubmitAnswerInATest(Guid userId, Guid testId, List<Guid> listAnswerIds)
        {
            foreach(var answerId in listAnswerIds)
            {
                var testAnswer = new TestAnswer { TestId = testId, AnswerId = answerId };
                await _unitOfWork.TestAnswerRepository.AddAsync(testAnswer, userId);
                await _unitOfWork.SaveChangesAsync();
            }

        }
        
    }
}
