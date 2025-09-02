using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        public IQueryable<Question> GetAllQuestionsAsync();
        public IQueryable<Question> GetQuestionByConditionPaging(Expression<Func<Question, bool>> predicate);
        public Task<Question> GetQuestionById(Guid id);
        public Task<IEnumerable<Question>> GetQuestionByCondition(Expression<Func<Question, bool>> predicate);
    }
}
