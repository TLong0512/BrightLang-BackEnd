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

namespace Infrastructure.Repositories.Implementations
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(DefaultContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            return await _context.Questions.Include(x => x.Context).ToListAsync();
        }

        public async Task<IEnumerable<Question>> GetQuestionByCondition(Expression<Func<Question, bool>> predicate)
        {
            return await _context.Questions.Include(x => x.Context).Where(predicate).ToListAsync();
        }

        public async Task<Question> GetQuestionById(Guid id)
        {
            return await _context.Questions.Include(x => x.Context).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
