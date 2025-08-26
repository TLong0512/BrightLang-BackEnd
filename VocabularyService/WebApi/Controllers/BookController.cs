using Application.Dtos.BookDto;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

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
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult> GetAllBooks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var books = await _bookService.GetAllBooksAsync(page, pageSize);
        return Ok(books);
    }

    // GET: api/book/{id}
    [Authorize(Roles = "User")]
    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetBookById(Guid id)
    {
        try
        {
            var book = await _bookService.GetBookByIdAsync(id);
            return Ok(book);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET: api/book/user/{userId}
    [HttpGet("user/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> GetBooksByUserIdForAdmin(Guid userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var books = await _bookService.GetAllBooksByUserIdAsync(userId, page, pageSize);
        return Ok(books);
    }

    // GET: api/book/user/myBook
    [HttpGet("myBook")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult> GetMyBook([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var books = await _bookService.GetMyBook(page, pageSize);
            return Ok(books);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST: api/book
    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<ActionResult> AddBook([FromBody] BookCreateDto bookCreateDto)
    {
        try
        {
            await _bookService.AddBookAsync(bookCreateDto);
            return Ok(new { message = "Book added successfully" });
        }
        catch (Exception ex) 
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/book
    [Authorize(Roles = "User")]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBook(Guid id, [FromBody] BookUpdateDto bookUpdateDto)
    {
        try
        {
            await _bookService.UpdateBookAsync(bookUpdateDto,id);
            return Ok(new { message = "Book updated successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // DELETE: api/book/{id}
    [Authorize(Roles = "User")]
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
