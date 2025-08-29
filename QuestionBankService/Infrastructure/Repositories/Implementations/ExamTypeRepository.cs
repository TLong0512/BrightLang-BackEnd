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
    public class ExamTypeRepository : GenericRepository<ExamType>, IExamTypeRepository
    {
        public ExamTypeRepository(DefaultContext context) : base(context)
        {
        }

        public async Task<ExamType> GetExamTypeByIdAsync(Guid id)
        {
            return await _context.ExamTypes.Include(x => x.Levels).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
