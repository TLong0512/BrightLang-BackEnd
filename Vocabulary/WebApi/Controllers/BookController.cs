using Application.Dtos.BookDto;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/book
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        // GET: api/book/{id}
        [Authorize(Roles = "user")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBookById(Guid id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                return Ok(book);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/book/user/{userId}
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<IEnumerable<BookOfUserDto>>> GetBooksByUserId(Guid userId)
        {
            var books = await _bookService.GetAllBooksByUserIdAsync(userId);
            return Ok(books);
        }

        // POST: api/book
        [Authorize(Roles = "user")]
        [HttpPost]
        public async Task<ActionResult> AddBook([FromBody] BookCreateDto bookCreateDto)
        {
            await _bookService.AddBookAsync(bookCreateDto);
            return Ok(new { message = "Book added successfully" });
        }

        // PUT: api/book
        [Authorize(Roles = "user")]
        [HttpPut]
        public async Task<ActionResult> UpdateBook([FromBody] BookUpdateDto bookUpdateDto)
        {
            try
            {
                await _bookService.UpdateBookAsync(bookUpdateDto);
                return Ok(new { message = "Book updated successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/book/{id}
        [Authorize(Roles = "user")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(Guid id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);
                return Ok(new { message = "Book deleted successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
