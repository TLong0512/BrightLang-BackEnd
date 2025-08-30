using Domain.Entities;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Intefaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    public class TestRepository : GenericRepository<Test>, ITestRepository
    {
        public TestRepository(DefaultContext context) : base(context)
        {
           
        }
        public IQueryable<Test> GetAllTestByConditionForPaging(Expression<Func<Test, bool>> predicate)
        {
            return _context.Set<Test>().Where(predicate).OrderByDescending(x => x.CreatedDate);

        }
    }
}
