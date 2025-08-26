using Domain.Entities;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Range = Domain.Entities.Range;

namespace Infrastructure.Repositories.Implementations
{
    public class RangeRepository : GenericRepository<Range>, IRangeRepository
    {
        public RangeRepository(DefaultContext context) : base(context)
        {
        }
    }
}
