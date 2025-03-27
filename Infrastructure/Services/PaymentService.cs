using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
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

        private readonly PaymentIntentService _service = new();

        public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
        {
            StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];

            var cart = await cartService.GetCartAsync(cartId);


            var shippingPrice = 0m;
            if (cart.DeliveryMethodId.HasValue)
            {
                var dm = await dmRepo.GetByIdAsync(cart.DeliveryMethodId.Value);
                if (dm is null) return null;

                shippingPrice = dm.Price;
            }

            decimal TotalPayment = 0;

            foreach (CartItem item in cart.Items)
            {
                var productSpec = new ProductPriceSpecification(item.ProductId);
                var productPrice = await productRepo.GetEntityWithSpecAsync(productSpec);

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
