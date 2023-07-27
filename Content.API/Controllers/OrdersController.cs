using Content.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Content.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly BookStore _bookStore;


        public OrdersController(BookStore bookStore)
        {
            _bookStore = bookStore;
        }

        [HttpGet]
        [Route("getOrders")]
        public IActionResult GetOrders()
        {
            return Ok();
        }
    }
}
