using Infrastructure.Contexts;
using Infrastructure.Repositories.Implementations;
using Infrastructure.Repositories.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DefaultContext _context;
        public ITestRepository TestRepository { get; }
        public ITestAnswerRepository TestAnswerRepository { get; }
        public ITestQuestionRepository TestQuestionRepository { get; }
        public UnitOfWork(DefaultContext context)
        {
            _context = context;
            TestRepository = new TestRepository(context);
            TestQuestionRepository = new TestQuestionRepository(context);
            TestAnswerRepository = new TestAnswerRepository(context);
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
