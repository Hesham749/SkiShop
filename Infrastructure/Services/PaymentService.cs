using Core.Entities;
using Core.Interfaces;
using Core.Specifications.ProductSpecifications;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly PaymentIntentService _service;
        private readonly IConfiguration _config;
        private readonly ICartService _cartService;
        private readonly IUnitOfWork _unit;

        public PaymentService(IConfiguration config, ICartService cartService, IUnitOfWork unit)
        {
            _config = config;
            _cartService = cartService;
            _unit = unit;
            _service = new();
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
        }

        public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
        {


            var cart = await _cartService.GetCartAsync(cartId);
            if (cart is null) return null;

            var shippingPrice = 0m;
            if (cart.DeliveryMethodId.HasValue)
            {
                var dm = await _unit.Repository<DeliveryMethod>().GetByIdAsync(cart.DeliveryMethodId.Value);
                if (dm is null) return null;

                shippingPrice = dm.Price;
            }

            decimal TotalPayment = 0;

            if (cart.Items.Count == 0) return null;

            foreach (CartItem item in cart.Items)
            {
                if (item is null) continue;

                var productSpec = new ProductPriceSpecification(item.ProductId);
                var productPrice = await _unit.Repository<Core.Entities.Product>()
                    .GetEntityWithSpecAsync(productSpec);

                if (!productPrice.HasValue) return null;

                item.Price = productPrice.Value;

                TotalPayment += (item.Price * 100 * item.Quantity);
            }

            PaymentIntent? intent;

            if (string.IsNullOrWhiteSpace(cart.PaymentIntentId)
                || !await PaymentIntentExists(cart.PaymentIntentId))
            {
                intent = await CreateIntentAsync(shippingPrice, TotalPayment);

                cart.PaymentIntentId = intent.Id;
            }
            else
            {
                intent = await UpdateIntentAsync(cart, shippingPrice, TotalPayment);
            }

            cart.ClientSecret = intent.ClientSecret;

            await _cartService.SetCartAsync(cart);
            return cart;

        }

        private async Task<bool> PaymentIntentExists(string paymentIntentId)
        {
            try
            {
                await _service.GetAsync(paymentIntentId);
                return true;
            }
            catch (Exception ex) when (ex.Message.Contains("No such payment_intent"))
            {
                return false;
            }
            catch
            {
                throw;
            }
        }

        private async Task<PaymentIntent> UpdateIntentAsync(ShoppingCart cart, decimal shippingPrice,
            decimal TotalPayment)
        {
            PaymentIntent? intent;
            var option = new PaymentIntentUpdateOptions
            {
                Amount = (long)(TotalPayment + shippingPrice),
            };

            intent = await _service.UpdateAsync(cart.PaymentIntentId, option);
            return intent;
        }

        private async Task<PaymentIntent> CreateIntentAsync(decimal shippingPrice,
            decimal TotalPayment)
        {
            PaymentIntent? intent;
            var option = new PaymentIntentCreateOptions
            {
                Amount = (long)(TotalPayment + shippingPrice),
                Currency = "usd",
                PaymentMethodTypes = ["card"]
            };

            intent = await _service.CreateAsync(option);

            return intent;
        }

        public async Task<string> RefundPayment(string PaymentIntentId)
        {
            var refundOption = new RefundCreateOptions
            {
                PaymentIntent = PaymentIntentId
            };

            var refundService = new RefundService();

            var result = await refundService.CreateAsync(refundOption);

            return result.Status;
        }
    }
}
