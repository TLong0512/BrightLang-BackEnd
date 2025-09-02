using Domain.Entities;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Intefaces
{
    public interface ITestRepository : IGenericRepository<Test>
    {
        public IQueryable<Test> GetAllTestByConditionForPaging(Expression<Func<Test, bool>> predicate);

    }
}
