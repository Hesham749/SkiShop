using Core.Interfaces;
using Order = Core.Entities.OrderAggregate.Order;
using Product = Core.Entities.Product;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(ICartService cartService, IUnitOfWork unit) : BaseApiController
    {
        private readonly ICartService _cartService = cartService;
        private readonly IUnitOfWork _unit = unit;

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto orderDto)
        {
            var email = User.GetUserEmail();

            var cart = await _cartService.GetCartAsync(orderDto.CartId);

            if (cart is null) return BadRequest("Cart not found");

            if (string.IsNullOrWhiteSpace(cart.PaymentIntentId))
                return BadRequest($"No payment intent for this order");


            var deliveryMethod = await _unit.Repository<DeliveryMethod>()
                .GetByIdAsync(orderDto.DeliveryMethodId!.Value);
            if (deliveryMethod is null) return BadRequest($"No delivery method was selected");

            var items = new List<OrderItem>();

            if (cart.Items.Count == 0) return BadRequest("No Items in the cart");

            foreach (var item in cart.Items)
            {
                if (item is null) continue;

                var product = await _unit.Repository<Product>().GetByIdAsync(item.ProductId);
                if (product is null) return BadRequest("Problem with the order");

                #region Updating cart item data
                item.PictureUrl = product.PictureUrl;
                item.Price = product.Price;
                item.Brand = product.Brand;
                #endregion

                var itemOrederd = new ProductItemOrdered
                {
                    ProductId = item.ProductId,
                    PictureUrl = product.PictureUrl,
                    ProductName = product.Name
                };

                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrederd,
                    Price = product.Price,
                    Quantity = item.Quantity
                };

                items.Add(orderItem);
            }

            await _cartService.SetCartAsync(cart);

            var order = new Order
            {
                BuyerEmail = email,
                OrderItems = items,
                PaymentIntentId = cart.PaymentIntentId,
                DeliveryMethod = deliveryMethod,
                PaymentSummary = orderDto.PaymentSummary,
                ShippingAddress = orderDto.ShippingAddress,
                Subtotal = items.Sum(i => i.Price * i.Quantity)
            };

            await _unit.Repository<Order>().AddAsync(order);

            if (await _unit.CompleteAsync())
                return CreatedAtAction(nameof(GetUserOrederById), new { order.Id }, order.ToDto());

            return BadRequest($"Problem creating order");
        }


        [HttpGet]
        public async Task<ActionResult<OrderDto>> GetOrdersForUser()
        {
            var spec = new OrderSpecification(User.GetUserEmail());

            var orders = await _unit.Repository<Order>().ListAsync(spec);

            return Ok(orders);

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetUserOrederById(int id)
        {
            var spec = new OrderSpecification(User.GetUserEmail(), id);

            var order = await _unit.Repository<Order>().GetEntityWithSpecAsync(spec);

            if (order is null) return NotFound();
            return order.ToDto();
        }

    }
}
