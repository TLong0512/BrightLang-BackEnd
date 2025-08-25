using Application.Dtos.BaseDto;
using Application.Dtos.BookDto;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IBookService
    {
        Task<PageResultDto<BookDto>> GetAllBooksAsync(int page,int  pageSize);
        Task<PageResultDto<BookOfUserDto>> GetAllBooksByUserIdAsync(Guid userId, int page, int pageSize);
        Task<PageResultDto<BookOfUserDto>> GetMyBook(int page, int pageSize);
        Task<BookDto> GetBookByIdAsync(Guid id);
        Task AddBookAsync(BookCreateDto bookCreateDto);
        Task UpdateBookAsync(BookUpdateDto bookUpdateDto, Guid id);
        Task DeleteBookAsync(Guid id);
    }
}
