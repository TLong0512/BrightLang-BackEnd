using Domain.Entities;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementations
{
    public class TestAnswerRepository : GenericRepository<TestAnswer>, ITestAnswerRepository
    {
        public TestAnswerRepository(DefaultContext context) : base(context)
        {

        }
    }
}
