using EduConnect.Controllers.Shopping;

namespace EduConnect.Interfaces.Shopping
{
    public interface IPaymentService
    {
        Task<ShoppingCart> CreateOrUpdatePaymentIntent(string cartID);
    }
}
