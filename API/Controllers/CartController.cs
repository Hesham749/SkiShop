using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ICartService cartService) : BaseApiController
    {
        private readonly ICartService _cartService = cartService;

        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetCartById([FromQuery] string id)
        {
            var cart = await _cartService.GetCartAsync(id);

            return Ok(cart ?? new() { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            var updatedCart = await _cartService.SetCartAsync(cart);
            if (updatedCart == null) return BadRequest("Problem with cart");

            return Ok(updatedCart);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCart([FromQuery] string id)
        {
            var Isdeleted = await _cartService.DeleteCartAsync(id);
            if (!Isdeleted) return BadRequest("Problem Deleting Cart");

            return NoContent();
        }
    }
}
