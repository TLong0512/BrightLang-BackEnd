using Domain.Entities;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Interfaces;
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
    }
}
