using Application.Dtos.AnswerDtos;
using Application.Dtos.BaseDtos;
using Application.Dtos.TestDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ITestService
    {
        public Task<PageResult<TestSummaryDto>> GetAllTestByUserIdAsync(Guid userId, int page = 1, int pageSize = 10);
        public Task<Guid> CreateTestAsync(Guid userId,  List<Guid> questionIds);
        public Task SubmitAnswerInATest(Guid userId, Guid testId, List<Guid> listAnswerIds);
    }
}
