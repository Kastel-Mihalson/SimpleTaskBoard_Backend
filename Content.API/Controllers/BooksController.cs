using Content.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Content.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookStore _bookStore;

        public BooksController(BookStore bookStore)
        {
            _bookStore = bookStore;
        }

        [HttpGet]
        [Route("getAvailableBooks")]
        public IActionResult GetAvailableBooks()
        {
            return Ok(_bookStore.Books);
        }
    }
}
