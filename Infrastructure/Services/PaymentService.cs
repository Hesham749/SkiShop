using Core.Entities;
using Core.Interfaces;
using Core.Specifications.ProductSpecifications;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services
{
    public class PaymentService(IConfiguration config, ICartService cartService,
        IUnitOfWork unit) : IPaymentService
    {

        private readonly PaymentIntentService _service = new();
        private readonly IUnitOfWork _unit = unit;

        public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
        {
            StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];

            var cart = await cartService.GetCartAsync(cartId);
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

            await cartService.SetCartAsync(cart);
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
    }
}
