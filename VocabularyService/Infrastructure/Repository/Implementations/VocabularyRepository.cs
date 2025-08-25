using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Implementations
{
    public class VocabularyRepository : GenericRepository<Vocabulary>, IVocabularyRepository
    {
        public VocabularyRepository(VocabularyContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Vocabulary>> GetAllVocabulariesByBookIdAsync(Guid bookId)
        {
            return await _context.Vocabularies
                .Where(v => v.BookId == bookId)
                .ToListAsync();
        }
    }
}
