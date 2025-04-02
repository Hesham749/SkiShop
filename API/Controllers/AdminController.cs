using Core.Interfaces;

namespace API.Controllers
{

    [Route("api/[Controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController(IUnitOfWork unit, IPaymentService paymentService) : BaseApiController
    {
        private readonly IUnitOfWork _unit = unit;
        private readonly IPaymentService _paymentService = paymentService;

        [HttpGet("orders")]
        public async Task<ActionResult<OrderDto>> GetOrders([FromQuery] OrderSpecParams specParam)
        {

            var specs = new OrderSpecification(specParam);

            return await CreatePagedResult(_unit.Repository<Order>(), specs
                , specParam.PageIndex, specParam.PageSize, o => o.ToDto());

        }

        [HttpGet("orders/{id:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var spec = new OrderSpecification(id);

            var order = await _unit.Repository<Order>().GetEntityWithSpecAsync(spec);

            if (order is null) return NotFound($"No order with id {id}");

            return Ok(order.ToDto());
        }

        [HttpGet("order/refund/{id:int}")]
        public async Task<ActionResult<OrderDto>> RefundOrder(int id)
        {
            var spec = new OrderSpecification(id);

            var order = await _unit.Repository<Order>().GetEntityWithSpecAsync(spec);

            if (order is null) return NotFound($"No order with id {id}");

            if (order.Status == OrderStatus.Pending) return BadRequest("Payment not received for this order");

            var result = await _paymentService.RefundPayment(order.PaymentIntentId);

            if (string.Equals(result, "succeeded", StringComparison.OrdinalIgnoreCase))
            {
                order.Status = OrderStatus.Refunded;
                await _unit.CompleteAsync();
                return order.ToDto();
            }

            return BadRequest("Problem Refunding Order");
        }

    }
}
