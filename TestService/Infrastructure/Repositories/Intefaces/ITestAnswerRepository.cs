using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Intefaces
{
    public interface ITestAnswerRepository : IGenericRepository<TestAnswer>
    {
        Task DeleteByCondition(Expression<Func<TestAnswer, bool>> predicate);
    }
}
