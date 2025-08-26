using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Range = Domain.Entities.Range;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IRangeRepository : IGenericRepository<Range>
    {
    }
}
