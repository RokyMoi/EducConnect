using EduConnect.Entities.Shopping;

namespace EduConnect.Interfaces.Shopping
{
    public interface IWishlistService
    {
        Task<Wishlist> CreateWishlistAsync(string email);
        Task<bool> AddCourseToWishlistAsync(string email, Guid courseId);
        Task<bool> RemoveCourseFromWishlistAsync(string email, Guid courseId);
        Task<bool> MoveCourseToShoppingCartAsync(string email, Guid courseId);
        Task<Wishlist?> GetWishlistForStudentAsync(string email);
    }
}
