using EduConnect.Entities.Shopping;

namespace EduConnect.Interfaces.Shopping
{
    public interface IWishListCourse
    {
        public Task<WishList> CreateWishListCartAsync(string email);
        public Task<bool> DeleteWishlistItemAsync(string email, Guid courseID);
        public Task<WishList?> GetWishListForStudentAsync(string email);
        public Task<bool> SetWishlistAsync(string email, Guid courseID);
    }
}
