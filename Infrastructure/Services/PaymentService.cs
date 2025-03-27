using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications.ProductSpecifications;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services
{
    public class PaymentService(IConfiguration config, ICartService cartService,
        IGenericRepository<Core.Entities.Product> productRepo, IGenericRepository<DeliveryMethod> dmRepo) : IPaymentService
    {
        public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
        {
            StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];

            var cart = await cartService.GetCartAsync(cartId);


            var shippingPrice = 0m;
            if (cart.DeliveryMethodId.HasValue)
            {
                var dm = await dmRepo.GetByIdAsync((int)cart.DeliveryMethodId);
                if (dm is null) return null;

                shippingPrice = dm.Price;
            }

            decimal TotalPayment = 0;

            foreach (CartItem item in cart.Items)
            {
                if (item is null) return null;  // ToDo maybe chang to continue

                var productSpec = new ProductPriceSpecification(item.ProductId);
                item.Price = await productRepo.GetEntityWithSpecAsync(productSpec);

                TotalPayment += (item.Price * 100 * item.Quantity);
            }

            var service = new PaymentIntentService();
            PaymentIntent? intent;

            if (string.IsNullOrWhiteSpace(cart.PaymentIntentId)
                || !await PaymentIntentExists(cart.PaymentIntentId, service))
            {
                intent = await CreateIntentAsync(shippingPrice,
                    TotalPayment, service);

                cart.PaymentIntentId = intent.Id;
            }
            else
            {
                intent = await UpdateIntentAsync(cart, shippingPrice,
                    TotalPayment, service);
            }

            cart.ClientSecret = intent.ClientSecret;

            await cartService.SetCartAsync(cart);
            return cart;

        }

        private static async Task<bool> PaymentIntentExists(string paymentIntentId, PaymentIntentService service)
        {
            try
            {
                await service.GetAsync(paymentIntentId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static async Task<PaymentIntent> UpdateIntentAsync(ShoppingCart cart, decimal shippingPrice,
            decimal TotalPayment, PaymentIntentService service)
        {
            PaymentIntent? intent;
            var option = new PaymentIntentUpdateOptions
            {
                Amount = (long)(TotalPayment + shippingPrice),
            };

            intent = await service.UpdateAsync(cart.PaymentIntentId, option);
            return intent;
        }

        private static async Task<PaymentIntent> CreateIntentAsync(decimal shippingPrice,
            decimal TotalPayment, PaymentIntentService service)
        {
            PaymentIntent? intent;
            var option = new PaymentIntentCreateOptions
            {
                Amount = (long)(TotalPayment + shippingPrice),
                Currency = "usd",
                PaymentMethodTypes = ["card"]
            };

            intent = await service.CreateAsync(option);

            return intent;
        }
    }
}
