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

        public async Task AddAnswerAsync(TestAnswer entity, Guid userId)
        {
            entity.CreatedDate = DateTime.Now;
            entity.CreatedBy = userId;

            //check whether answer has been deleted in db
            var deletedItem = await _context.TestAnswers
                            .IgnoreQueryFilters()
                            .FirstOrDefaultAsync(x => x.TestId == entity.TestId && x.AnswerId == entity.AnswerId);
            if (deletedItem!= null)
            {
                deletedItem.IsDeleted = false;
                deletedItem.CreatedDate = DateTime.Now;
                deletedItem.CreatedBy = userId;
                deletedItem.UpdatedDate = null;
                deletedItem.UpdatedBy = null;
            }
            else
            {
                await _context.TestAnswers.AddAsync(entity);
            }
        }

        public async Task DeleteByCondition(Expression<Func<TestAnswer, bool>> predicate)
        {
            await _context.TestAnswers
                       .Where(predicate)
                       .ExecuteUpdateAsync(s => s.SetProperty(e => e.IsDeleted, true));
        }
    }
}
