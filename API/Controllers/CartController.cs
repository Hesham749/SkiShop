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
            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            var UpdatedCart = await _cartService.SetCartAsync(cart);
            return Ok(UpdatedCart);
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
