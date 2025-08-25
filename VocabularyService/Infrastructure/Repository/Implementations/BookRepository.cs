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
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(VocabularyContext context) : base(context)
        {
        }

        public IQueryable<Book> GetAllBooksByUserIdAsync(Guid userId)
        {
            return _context.Books
                .Where(b => b.UserId == userId);
        }

        public IQueryable<Book> GetAllForPaging()
        {
            return _context.Set<Book>();
        }
    }
}
