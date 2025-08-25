using Application.Dtos.BookDto;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddBookAsync(BookCreateDto bookCreateDto)
        {
            var book = _mapper.Map<Book>(bookCreateDto);
            await _unitOfWork.Books.AddAsync(book);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(Guid id)
        {
           var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }
            else
            {
                await _unitOfWork.Books.Delete(book);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = await _unitOfWork.Books.GetAllAsync();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<IEnumerable<BookOfUserDto>> GetAllBooksByUserIdAsync(Guid userId)
        {
            var books = await _unitOfWork.Books.GetAllBooksByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<BookOfUserDto>>(books);
        }

        public async Task<BookDto> GetBookByIdAsync(Guid id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }
            else
            {
                return _mapper.Map<BookDto>(book);
            }
        }

        public async Task UpdateBookAsync(BookUpdateDto bookUpdateDto)
        {
            var bookUpdate = await _unitOfWork.Books.GetByIdAsync(bookUpdateDto.Id);
            if (bookUpdate == null)
            {
                throw new KeyNotFoundException("Book not found");
            }
            else
            {
                var book = _mapper.Map<Book>(bookUpdateDto);
                await _unitOfWork.Books.Update(book);
                var result = await _unitOfWork.SaveChangesAsync();
                return;
            }
        }
    }
}
