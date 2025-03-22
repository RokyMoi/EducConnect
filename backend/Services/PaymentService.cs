using EduConnect.Controllers.Shopping;
using EduConnect.Data;
using EduConnect.Entities.Shopping;
using EduConnect.Interfaces.Shopping;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Diagnostics.Eventing.Reader;

namespace EduConnect.Services
{
    public class PaymentService(IConfiguration config,IShoppingCartService CartService,DataContext db) : IPaymentService
    {
        public async Task<bool> CreateOrUpdatePaymentIntent(Guid cartID)
        {
            StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
            var shoppingCart = await db.ShoppingCart.Where(x => x.ShoppingCartID == cartID).FirstOrDefaultAsync();
            if (shoppingCart == null)
            {
                return false;
            }

            var service = new PaymentIntentService();
            PaymentIntent? intent = null;

            if (string.IsNullOrEmpty(shoppingCart.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = await CartService.GetTotalPriceAsync(cartID),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                intent = await service.CreateAsync(options);
                shoppingCart.PaymentIntentId = intent.Id;
                shoppingCart.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = await CartService.GetTotalPriceAsync(cartID)
                };
                intent = await service.UpdateAsync(shoppingCart.PaymentIntentId, options);
            }

            // Ažuriraj bazu podataka
            db.ShoppingCart.Update(shoppingCart);
            await db.SaveChangesAsync();

            return true;
        }

        Task<ShoppingCart?> IPaymentService.CreateOrUpdatePaymentIntent(Guid cartID)
        {
            throw new NotImplementedException();
        }
    }
}
