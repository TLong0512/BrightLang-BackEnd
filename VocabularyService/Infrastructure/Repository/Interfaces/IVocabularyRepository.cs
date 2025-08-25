using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Interfaces
{
    public interface IVocabularyRepository : IGenericRepository<Vocabulary>
    {
        Task<IEnumerable<Vocabulary>> GetAllVocabulariesByBookIdAsync(Guid bookId);
    }
}
