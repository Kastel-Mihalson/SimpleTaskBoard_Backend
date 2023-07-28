using Content.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize]
        [HttpGet]
        [Route("getOrders")]
        public IActionResult GetOrders()
        {
            var userId = User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var orderIds = _bookStore.Orders.SingleOrDefault(o => o.Key == Guid.Parse(userId)).Value;
            if (orderIds is null)
            {
                return Ok("Empty order");
            }

            var orderBooks = _bookStore.Books.Where(b => orderIds.Contains(b.Id));
            return Ok(orderBooks);
        }
    }
}
