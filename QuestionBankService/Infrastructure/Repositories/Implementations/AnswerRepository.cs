using Domain.Entities;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(DefaultContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Answer>> GetAllAnswersAsync()
        {
            return await _context.Answers.Include(x => x.Question).ToListAsync();
        }

        public async Task<Answer> GetAnswerById(Guid id)
        {
            return await _context.Answers.Include(x => x.Question).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
