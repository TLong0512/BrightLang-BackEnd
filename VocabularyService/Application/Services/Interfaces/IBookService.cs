using Application.Dtos.BookDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync();
        Task<IEnumerable<BookOfUserDto>> GetAllBooksByUserIdAsync(Guid userId);
        Task<BookDto> GetBookByIdAsync(Guid id);
        Task AddBookAsync(BookCreateDto bookCreateDto);
        Task UpdateBookAsync(BookUpdateDto bookUpdateDto);
        Task DeleteBookAsync(Guid id);
    }
}
