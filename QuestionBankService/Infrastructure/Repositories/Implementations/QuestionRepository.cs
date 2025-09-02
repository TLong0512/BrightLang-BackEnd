using Domain.Entities;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.Repositories.Implementations
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(DefaultContext context) : base(context)
        {
        }
        public IQueryable<Question> GetAllQuestionsAsync()
        {
            return _context.Set<Question>();
        }
        public IQueryable<Question> GetQuestionByConditionPaging(Expression<Func<Question, bool>> predicate)
        {
            return _context.Set<Question>().Where(predicate).OrderByDescending(x => x.CreatedDate);
        }

        public async Task<IEnumerable<Question>> GetQuestionByCondition(Expression<Func<Question, bool>> predicate)
        {
            return await _context.Questions.Include(x => x.Context).Where(predicate).ToListAsync();
        }

        public async Task<Question> GetQuestionById(Guid id)
        {
            return await _context.Questions.Include(x => x.Context).Include(x => x.Answers).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
