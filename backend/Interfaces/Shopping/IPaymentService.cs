
using EduConnect.Entities.Shopping;

namespace EduConnect.Interfaces.Shopping
{
    public interface IPaymentService
    {
        Task<ShoppingCart?> CreateOrUpdatePaymentIntent(Guid cartID);
    }
}
