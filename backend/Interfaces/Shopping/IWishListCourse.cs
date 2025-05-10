using EduConnect.Entities.Shopping;
using System;
using System.Threading.Tasks;

namespace EduConnect.Interfaces.Shopping
{
    /// <summary>
    /// Interface for wishlist operations
    /// </summary>
    public interface IWishlistService
    {
        /// <summary>
        /// Gets the wishlist for a student by email
        /// </summary>
        /// <param name="email">Student email</param>
        /// <returns>Wishlist or null if not found</returns>
        Task<Wishlist?> GetWishlistForStudentAsync(string email);

        /// <summary>
        /// Adds a course to the student's wishlist
        /// </summary>
        /// <param name="email">Student email</param>
        /// <param name="courseId">Course ID to add</param>
        /// <returns>True if added successfully, false if already in wishlist</returns>
        Task<bool> AddCourseToWishlistAsync(string email, Guid courseId);

        /// <summary>
        /// Removes a course from the student's wishlist
        /// </summary>
        /// <param name="email">Student email</param>
        /// <param name="courseId">Course ID to remove</param>
        /// <returns>True if removed successfully, false if not found</returns>
        Task<bool> RemoveCourseFromWishlistAsync(string email, Guid courseId);

        /// <summary>
        /// Checks if a course is in the student's wishlist
        /// </summary>
        /// <param name="email">Student email</param>
        /// <param name="courseId">Course ID to check</param>
        /// <returns>True if course is in wishlist, false otherwise</returns>
        Task<bool> IsCourseInWishlistAsync(string email, Guid courseId);

        /// <summary>
        /// Gets the number of items in the student's wishlist
        /// </summary>
        /// <param name="email">Student email</param>
        /// <returns>Number of items in wishlist</returns>
        Task<int> GetItemCountAsync(string email);

        /// <summary>
        /// Clears all items from the student's wishlist
        /// </summary>
        /// <param name="email">Student email</param>
        /// <returns>True if cleared successfully</returns>
        Task<bool> ClearWishlistAsync(string email);
    }
}