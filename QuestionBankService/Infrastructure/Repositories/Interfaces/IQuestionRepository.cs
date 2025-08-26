using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        public Task<IEnumerable<Question>> GetAllQuestionsAsync();
        public Task<Question> GetQuestionById(Guid id);
        public Task<IEnumerable<Question>> GetQuestionByCondition(Expression<Func<Question, bool>> predicate);
    }
}
