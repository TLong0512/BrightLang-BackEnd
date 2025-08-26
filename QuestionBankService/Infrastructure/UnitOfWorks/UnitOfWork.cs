using Domain.Entities;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Implementations;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
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

        public IAnswerRepository AnswerRepository { get; }
        public IContextRepository ContextRepository { get; }
        public IExamTypeRepository ExamTypeRepository { get; }
        public ILevelRepository LevelRepository { get; }
        public IQuestionRepository QuestionRepository { get; }
        public IRangeRepository RangeRepository { get; }
        public ISkillLevelRepository SkillLevelRepository { get; }
        public ISkillRepository SkillRepository { get; }

        // Constructor
        public UnitOfWork(DefaultContext context)
        {
            _context = context;

            AnswerRepository = new AnswerRepository(context);
            ContextRepository = new ContextRepository(context);
            ExamTypeRepository = new ExamTypeRepository(context);
            LevelRepository = new LevelRepository(context);
            QuestionRepository = new QuestionRepository(context);
            RangeRepository = new RangeRepository(context);
            SkillLevelRepository = new SkillLevelRepository(context);
            SkillRepository = new SkillRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {

            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
