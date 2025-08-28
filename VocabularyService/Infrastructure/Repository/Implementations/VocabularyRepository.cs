using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Implementations
{
    public class VocabularyRepository : GenericRepository<Vocabulary>, IVocabularyRepository
    {
        public VocabularyRepository(VocabularyContext context) : base(context)
        {
        }

        public IQueryable<Vocabulary> GetAllForPaging()
        {
            return _context.Set<Vocabulary>();
        }

        public IQueryable<Vocabulary> GetAllVocabulariesByBookIdAsync(Guid bookId)
        {
            return _context.Vocabularies
                .Where(v => v.BookId == bookId);
        }

        public Guid? GetUserIdByBookId(Guid bookId)
        {
            return (from item in _context.Books
                    where item.Id == bookId
                    select (Guid?)item.UserId)
           .FirstOrDefault();
        }

        public Guid GetUserIdByVocabularyId(Guid vocabularyId)
        {
            return _context.Vocabularies
                .Where(v => v.Id == vocabularyId)
                .Select(v => v.Book!.UserId)
                .FirstOrDefault();
        }
    }
}
