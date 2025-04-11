using EduConnect.Entities.Course;
using EduConnect.Entities.Shopping;

namespace EduConnect.Interfaces.Shopping
{
    public interface IShoppingCartService
    {
      
            Task<ShoppingCart> CreateShoppingCartAsync(string email);
            Task<bool> DeleteShoppingCartItemAsync(string email, Guid courseID);
            Task<ShoppingCart?> GetShoppingCartForStudentAsync(string email);
            Task<decimal> GetTotalPriceAsync(Guid shoppingCartId);
            Task<bool> AddCourseToShoppingCartAsync(string email, Guid courseID);
            Task<bool> MoveCourseToWishListAsync(string email, Guid courseId);
            Task<bool> ClearShoppingCartAsync(string email);
        Task<ShoppingCart?> GetShoppingCartByIdAsync(Guid cartId);
            Task<int> GetItemCountAsync(string email);
        


    }
}
