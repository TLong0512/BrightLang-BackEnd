using Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IAnswerRepository AnswerRepository { get;}    
        IContextRepository ContextRepository { get; }
        IExamTypeRepository ExamTypeRepository { get; }
        ILevelRepository LevelRepository { get; }  
        IQuestionRepository QuestionRepository { get; }
        IRangeRepository RangeRepository { get;  }  
        ISkillLevelRepository SkillLevelRepository { get; }
        ISkillRepository SkillRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
