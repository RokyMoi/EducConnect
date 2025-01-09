using EduConnect.Controllers.Shopping;
using EduConnect.Interfaces.Shopping;
using Stripe;

namespace EduConnect.Services
{
    public class PaymentService(IConfiguration config,IShoppingCartService CartService) : IPaymentService
    {
        public async Task<ShoppingCart> CreateOrUpdatePaymentIntent(string cartID)
        {
            StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
        }
    }
}
