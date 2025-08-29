using Infrastructure.Repositories.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        ITestRepository TestRepository { get;}
        ITestAnswerRepository TestAnswerRepository { get; }
        ITestQuestionRepository TestQuestionRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
