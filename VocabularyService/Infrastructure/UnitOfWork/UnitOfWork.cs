using Infrastructure.Context;
using Infrastructure.Repository.Implementations;
using Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly VocabularyContext _context;

        public IBookRepository Books { get; }
        public IVocabularyRepository Vocabularies { get; }

        public UnitOfWork(VocabularyContext context)
        {
            _context = context;
            Books = new BookRepository(_context);
            Vocabularies = new VocabularyRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
