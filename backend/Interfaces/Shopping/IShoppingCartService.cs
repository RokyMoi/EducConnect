using EduConnect.Entities.Course;
using EduConnect.Entities.Shopping;
using System;
using System.Threading.Tasks;

namespace EduConnect.Interfaces.Shopping
{
    /// <summary>
    /// Interface for shopping cart operations
    /// </summary>
    public interface IShoppingCartService
    {
        /// <summary>
        /// Creates a shopping cart for a student
        /// </summary>
        /// <param name="email">Student email</param>
        /// <returns>Shopping cart entity</returns>
        Task<ShoppingCart> CreateShoppingCartAsync(string email);

        /// <summary>
        /// Removes a course from student's shopping cart
        /// </summary>
        /// <param name="email">Student email</param>
        /// <param name="courseId">Course ID to remove</param>
        /// <returns>True if removed successfully, false otherwise</returns>
        Task<bool> DeleteShoppingCartItemAsync(string email, Guid courseId);

        /// <summary>
        /// Gets the shopping cart for a student with full course details
        /// </summary>
        /// <param name="email">Student email</param>
        /// <returns>Shopping cart with courses or null if not found</returns>
        Task<ShoppingCart?> GetShoppingCartForStudentAsync(string email);

        /// <summary>
        /// Calculates total price of all courses in the shopping cart
        /// </summary>
        /// <param name="shoppingCartId">Shopping cart ID</param>
        /// <returns>Total price as decimal</returns>
        Task<decimal> GetTotalPriceAsync(Guid shoppingCartId);

        /// <summary>
        /// Adds a course to student's shopping cart
        /// </summary>
        /// <param name="email">Student email</param>
        /// <param name="courseId">Course ID to add</param>
        /// <returns>True if added successfully, false if already in cart</returns>
        Task<bool> AddCourseToShoppingCartAsync(string email, Guid courseId);

        /// <summary>
        /// Moves a course from shopping cart to wishlist
        /// </summary>
        /// <param name="email">Student email</param>
        /// <param name="courseId">Course ID to move</param>
        /// <returns>True if moved successfully, false otherwise</returns>
        Task<bool> MoveCourseToWishListAsync(string email, Guid courseId);

        /// <summary>
        /// Removes all courses from student's shopping cart
        /// </summary>
        /// <param name="email">Student email</param>
        /// <returns>True if cleared successfully, false otherwise</returns>
        Task<bool> ClearShoppingCartAsync(string email);

        /// <summary>
        /// Gets shopping cart by ID with course details
        /// </summary>
        /// <param name="cartId">Shopping cart ID</param>
        /// <returns>Shopping cart with courses or null if not found</returns>
        Task<ShoppingCart?> GetShoppingCartByIdAsync(Guid cartId);

        /// <summary>
        /// Gets number of items in student's shopping cart
        /// </summary>
        /// <param name="email">Student email</param>
        /// <returns>Number of items in cart</returns>
        Task<int> GetItemCountAsync(string email);

        /// <summary>
        /// Checks if a specific course is in the student's shopping cart
        /// </summary>
        /// <param name="email">Student email</param>
        /// <param name="courseId">Course ID to check</param>
        /// <returns>True if course is in cart, false otherwise</returns>
        Task<bool> IsCourseInCartAsync(string email, Guid courseId);
    }
}