using EduConnect.Entities.Course;
using EduConnect.Entities.Shopping;

namespace EduConnect.Interfaces.Shopping
{
    public interface IShoppingCartService
    {
        public Task<ShoppingCart> CreateShoppingCartAsync(string email);
        public Task<bool> DeleteShoppingCartItemAsync(string email, Guid courseID);
        public Task<ShoppingCart?> GetShoppingCartForStudentAsync(string email);
        public Task<bool> SetShoppingCartAsync(string email, Guid courseID);
        public  Task<long> GetTotalPriceAsync(Guid shoppingCartId);


    }
}
