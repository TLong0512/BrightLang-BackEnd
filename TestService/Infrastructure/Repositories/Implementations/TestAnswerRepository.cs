using Domain.Entities;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Intefaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    public class TestAnswerRepository : GenericRepository<TestAnswer>, ITestAnswerRepository
    {
        public TestAnswerRepository(DefaultContext context) : base(context)
        {

        }

        public Task DeleteByCondition(Expression<Func<TestAnswer, bool>> predicate)
        {
            _context.TestAnswers
                       .Where(predicate)
                       .ExecuteUpdateAsync(s => s.SetProperty(e => e.IsDeleted, true));
            return Task.CompletedTask;
        }
    }
}
