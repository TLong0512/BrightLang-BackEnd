using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface ILevelRepository : IGenericRepository<Level>
    {
        public Task<IEnumerable<Level>> GetAllLevelsAsync();
        public Task<Level> GetLevelByIdAsync(Guid id);
    }
}
