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
        IQueryable<Vocabulary> GetAllVocabulariesByBookIdAsync(Guid bookId);
        IQueryable<Vocabulary> GetAllForPaging();
        Guid? GetUserIdByBookId(Guid bookId);
        Guid GetUserIdByVocabularyId(Guid vocabularyId);
    }
}
