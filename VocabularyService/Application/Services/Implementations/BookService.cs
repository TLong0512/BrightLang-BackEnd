using Application.Dtos.BaseDto;
using Application.Dtos.BookDto;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task AddBookAsync(BookCreateDto bookCreateDto)
        {
            var book = _mapper.Map<Book>(bookCreateDto);

            Guid? userId = _currentUserService.UserId;

            if (userId == null || userId == Guid.Empty)
            {
                throw new Exception("Your token is broken.");
            }

            book.Id = Guid.NewGuid();
            book.UserId = (Guid)userId;
            
            await _unitOfWork.Books.AddAsync(book);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(Guid id)
        {
            Guid? userId = _currentUserService.UserId;

            if (userId == null || userId == Guid.Empty)
            {
                throw new Exception("Your token is broken.");
            }

            var book = await _unitOfWork.Books.GetByIdAsync(id);

            if (book.UserId != userId)
            {
                throw new Exception("You cant delete this book if you don't own it.");
            }
            
            else if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            else
            {
                await _unitOfWork.Books.Delete(book);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<PageResultDto<BookDto>> GetAllBooksAsync(int page, int pagesize)
        {
            var query = _unitOfWork.Books.GetAllForPaging();

            var totalItem = query.Count();

            var books = query
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToList();

            return new PageResultDto<BookDto> {
                Items = _mapper.Map<List<BookDto>>(books),
                Page = page,
                PageSize = pagesize,
                TotalItems = totalItem
            };
        }

        public async Task<PageResultDto<BookOfUserDto>> GetAllBooksByUserIdAsync(Guid userId, int page, int pagesize)
        {
            var query =  _unitOfWork.Books.GetAllBooksByUserIdAsync(userId);

            var totalItem = query.Count();

            var books = query
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToList();

            return new PageResultDto<BookOfUserDto>
            {
                Items = _mapper.Map<List<BookOfUserDto>>(books),
                Page = page,
                PageSize = pagesize,
                TotalItems = totalItem
            };
        }

        public async Task<BookDto> GetBookByIdAsync(Guid id)
        {
            Guid? userId = _currentUserService.UserId;

            if (userId == null || userId == Guid.Empty)
            {
                throw new Exception("Your token is broken.");
            }

            var book = await _unitOfWork.Books.GetByIdAsync(id);

            if (book == null)
            {
                throw new Exception("Book not found");
            }
            else if (book.UserId != userId)
            {
                throw new Exception("You cant view this book if you don't own it.");
            }
            else
            {
                return _mapper.Map<BookDto>(book);
            }
        }

        public async Task<PageResultDto<BookOfUserDto>> GetMyBook(int page, int pagesize)
        {
            Guid? userId = _currentUserService.UserId;

            if (userId == null || userId == Guid.Empty)
            {
                throw new Exception("Your token is broken.");
            }

            var query = _unitOfWork.Books.GetAllBooksByUserIdAsync((Guid)userId);

            var totalItem = query.Count();

            var books = query
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToList();

            return new PageResultDto<BookOfUserDto>
            {
                Items = _mapper.Map<List<BookOfUserDto>>(books),
                Page = page,
                PageSize = pagesize,
                TotalItems = totalItem
            };
        }

        public async Task UpdateBookAsync(BookUpdateDto bookUpdateDto, Guid id)
        {
            Guid? userId = _currentUserService.UserId;

            if (userId == null || userId == Guid.Empty)
            {
                throw  new Exception("Your token is broken.");
            }

            var bookUpdate = await _unitOfWork.Books.GetByIdAsync(id);

            if (bookUpdate.UserId != userId)
            {
                throw new Exception("You cant edit this book if you don't own it.");
            }
            else if (bookUpdate == null)
            {
                throw new Exception("Book not found");
            }
            else
            {
                var book = _mapper.Map<Book>(bookUpdateDto);
                book.Id = id;
                book.UserId = (Guid)userId;
                await _unitOfWork.Books.Update(book);
                var result = await _unitOfWork.SaveChangesAsync();
                return;
            }
        }
    }
}
