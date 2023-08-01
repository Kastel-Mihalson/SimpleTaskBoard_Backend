using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleTaskBoard.Domain.Models;
using SimpleTaskBoard.Infrastructure.Interfaces;
using System.Security.Claims;

namespace Content.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private IRepositoryWrapper _repository;

        public BooksController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [Authorize]
        [HttpGet("get-all-books")]
        public async Task<IActionResult> GetAllBooks()
        {
            var userId = User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var books = await _repository.Book.GetAllBooks();

            return Ok(books);
        }

        [Authorize]
        [HttpPost("add-book")]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            _repository.Book.Create(new Book
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
            });
            await _repository.SaveAsync();

            return Ok("Book created!");
        }

        
    }
}
