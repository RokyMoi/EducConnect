using EduConnect.Data;
using EduConnect.Entities.Course;
using EduConnect.Entities.Shopping;
using EduConnect.Entities.Student;
using EduConnect.Interfaces.Shopping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Services
{
    /// <summary>
    /// Service for managing shopping cart operations
    /// </summary>
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly DataContext _context;
        private readonly IWishlistService _wishlistService;
        private readonly ILogger<ShoppingCartService> _logger;

        public ShoppingCartService(
            DataContext context,
            IWishlistService wishlistService,
            ILogger<ShoppingCartService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _wishlistService = wishlistService ?? throw new ArgumentNullException(nameof(wishlistService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get student details by email
        /// </summary>
        /// <param name="email">Student email</param>
        /// <returns>Student entity</returns>
        /// <exception cref="ArgumentException">Thrown when email is invalid or student not found</exception>
        private async Task<Student> GetStudentByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            _logger.LogInformation("Getting student for email: {Email}", email);

            var person = await _context.PersonEmail
                .FirstOrDefaultAsync(x => x.Email == email);

            if (person == null)
            {
                _logger.LogWarning("Person with email {Email} not found", email);
                throw new ArgumentException("Cannot read email from token sent", nameof(email));
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(x => x.PersonId == person.PersonId);

            if (student == null)
            {
                _logger.LogWarning("Student not found for person with ID {PersonId}", person.PersonId);
                throw new ArgumentException("Student not found", nameof(email));
            }

            return student;
        }

        /// <summary>
        /// Gets existing shopping cart or creates a new one if it doesn't exist
        /// </summary>
        /// <param name="email">Student email</param>
        /// <param name="includeDetails">Whether to include course details</param>
        /// <returns>Shopping cart entity</returns>
        private async Task<ShoppingCart> GetOrCreateShoppingCartAsync(string email, bool includeDetails = false)
        {
            var student = await GetStudentByEmailAsync(email);

            _logger.LogInformation("Getting or creating shopping cart for student ID: {StudentId}", student.StudentId);

            IQueryable<ShoppingCart> query = _context.ShoppingCart
                .Include(sc => sc.Items);

            if (includeDetails)
            {
                query = query.Include(sc => sc.Items)
                             .ThenInclude(item => item.Course)
                             .ThenInclude(c => c.CourseDetails);
            }

            var shoppingCart = await query.FirstOrDefaultAsync(x => x.StudentID == student.StudentId);

            if (shoppingCart == null)
            {
                _logger.LogInformation("Creating new shopping cart for student ID: {StudentId}", student.StudentId);

                shoppingCart = new ShoppingCart
                {
                    ShoppingCartID = Guid.NewGuid(),
                    StudentID = student.StudentId,
                    Student = student,
                    Items = new List<ShoppingCartItem>(),
                    CreatedAt = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow
                };

                _context.ShoppingCart.Add(shoppingCart);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Created new shopping cart with ID: {CartId}", shoppingCart.ShoppingCartID);
            }

            return shoppingCart;
        }

        /// <summary>
        /// Creates a shopping cart for a student
        /// </summary>
        /// <param name="email">Student email</param>
        /// <returns>Shopping cart entity</returns>
        public async Task<ShoppingCart> CreateShoppingCartAsync(string email)
        {
            _logger.LogInformation("Explicitly creating shopping cart for email: {Email}", email);
            return await GetOrCreateShoppingCartAsync(email, includeDetails: true);
        }

        /// <summary>
        /// Removes a course from student's shopping cart
        /// </summary>
        /// <param name="email">Student email</param>
        /// <param name="courseId">Course ID to remove</param>
        /// <returns>True if removed successfully, false otherwise</returns>
        public async Task<bool> DeleteShoppingCartItemAsync(string email, Guid courseId)
        {
            try
            {
                _logger.LogInformation("Removing course {CourseId} from cart for email: {Email}", courseId, email);

                var shoppingCart = await GetOrCreateShoppingCartAsync(email);

                var course = await _context.Course.FindAsync(courseId);
                if (course == null)
                {
                    _logger.LogWarning("Course with ID {CourseId} not found", courseId);
                    return false;
                }

                var cartItem = shoppingCart.Items.FirstOrDefault(item => item.CourseID == courseId);
                if (cartItem != null)
                {
                    shoppingCart.Items.Remove(cartItem);
                    _context.ShoppingCartItem.Remove(cartItem);

                    shoppingCart.LastModified = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Course {CourseId} removed successfully", courseId);
                    return true;
                }
                else
                {
                    _logger.LogInformation("Course {CourseId} was not in the cart", courseId);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing course {CourseId} from cart for {Email}", courseId, email);
                throw;
            }
        }

        /// <summary>
        /// Gets the shopping cart for a student with full course details
        /// </summary>
        /// <param name="email">Student email</param>
        /// <returns>Shopping cart with courses or null if not found</returns>
        public async Task<ShoppingCart?> GetShoppingCartForStudentAsync(string email)
        {
            try
            {
                _logger.LogInformation("Getting shopping cart for email: {Email}", email);

                var student = await GetStudentByEmailAsync(email);

                return await _context.ShoppingCart
                    .Include(sc => sc.Items)
                        .ThenInclude(item => item.Course)
                        .ThenInclude(c => c.CourseDetails)
                    .FirstOrDefaultAsync(x => x.StudentID == student.StudentId);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Failed to get shopping cart for email: {Email}", email);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving shopping cart for {Email}", email);
                throw;
            }
        }

        /// <summary>
        /// Calculates total price of all courses in the shopping cart
        /// </summary>
        /// <param name="shoppingCartId">Shopping cart ID</param>
        /// <returns>Total price as decimal</returns>
        /// <exception cref="ArgumentException">Thrown when shopping cart not found</exception>
        public async Task<decimal> GetTotalPriceAsync(Guid shoppingCartId)
        {
            try
            {
                _logger.LogInformation("Calculating total price for cart: {CartId}", shoppingCartId);

                var shoppingCart = await _context.ShoppingCart
                    .Include(x => x.Items)
                        .ThenInclude(item => item.Course)
                        .ThenInclude(c => c.CourseDetails)
                    .FirstOrDefaultAsync(x => x.ShoppingCartID == shoppingCartId);

                if (shoppingCart == null)
                {
                    _logger.LogWarning("Shopping cart with ID {CartId} not found", shoppingCartId);
                    throw new ArgumentException("Shopping cart not found", nameof(shoppingCartId));
                }

                if (!shoppingCart.Items.Any())
                {
                    _logger.LogInformation("Shopping cart {CartId} is empty", shoppingCartId);
                    return 0m;
                }

                var totalPrice = shoppingCart.Items.Sum(item => item.Course.Price);

                _logger.LogInformation("Total price for cart {CartId}: {TotalPrice}", shoppingCartId, totalPrice);

                return totalPrice;
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                _logger.LogError(ex, "Error calculating total price for cart {CartId}", shoppingCartId);
                throw;
            }
        }

        /// <summary>
        /// Adds a course to student's shopping cart
        /// </summary>
        /// <param name="email">Student email</param>
        /// <param name="courseId">Course ID to add</param>
        /// <returns>True if added successfully, false if already in cart</returns>
        /// <exception cref="ArgumentException">Thrown when course not found</exception>
        public async Task<bool> AddCourseToShoppingCartAsync(string email, Guid courseId)
        {
            try
            {
                var shoppingCart = await GetOrCreateShoppingCartAsync(email);

                // Proveri da li je kurs već u listi želja
                if (shoppingCart.Items.Any(item => item.CourseID == courseId))
                {
                    return false; // Već postoji u listi
                }

                // Proveri da li kurs postoji
                var course = await _context.Course
                    .Include(c => c.CourseDetails)
                    .FirstOrDefaultAsync(c => c.CourseId == courseId);

                if (course == null)
                {
                    throw new ArgumentException("Course not found", nameof(courseId));
                }

                // Kreiraj novi WishlistItem
                var shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartItemID = Guid.NewGuid(),
                    ShoppingCartID = shoppingCart.ShoppingCartID,
                    CourseID = courseId,
                    Course = course,
                    AddedAt = DateTime.UtcNow
                };

                // Direktno dodaj novi red u kontekst (ne preko kolekcije)
                _context.ShoppingCartItem.Add(shoppingCartItem);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                _logger.LogError(ex, "Error adding course {CourseId} to wishlist for {Email}", courseId, email);
                throw;
            }
        }
        /// <summary>
        /// Moves a course from shopping cart to wishlist
        /// </summary>
        /// <param name="email">Student email</param>
        /// <param name="courseId">Course ID to move</param>
        /// <returns>True if moved successfully, false otherwise</returns>
        /// <exception cref="ArgumentException">Thrown when course not found</exception>
        public async Task<bool> MoveCourseToWishListAsync(string email, Guid courseId)
        {
            try
            {
                _logger.LogInformation("Moving course {CourseId} to wishlist for email: {Email}", courseId, email);

                var shoppingCart = await GetOrCreateShoppingCartAsync(email);

                var course = await _context.Course.FindAsync(courseId);
                if (course == null)
                {
                    _logger.LogWarning("Course with ID {CourseId} not found", courseId);
                    throw new ArgumentException("Course not found", nameof(courseId));
                }

                // Check if course is in cart
                var cartItem = shoppingCart.Items.FirstOrDefault(item => item.CourseID == courseId);
                if (cartItem == null)
                {
                    _logger.LogWarning("Course {CourseId} not found in cart", courseId);
                    return false;
                }

                // Remove from cart
                shoppingCart.Items.Remove(cartItem);
                _context.ShoppingCartItem.Remove(cartItem);
                shoppingCart.LastModified = DateTime.UtcNow;

                // Add to wishlist 
                bool addedToWishList = await _wishlistService.AddCourseToWishlistAsync(email, courseId);

                if (addedToWishList)
                {
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Course {CourseId} moved to wishlist successfully", courseId);
                    return true;
                }

                // If adding to wishlist fails, add back to cart
                var newCartItem = new ShoppingCartItem
                {
                    ShoppingCartItemID = Guid.NewGuid(),
                    ShoppingCartID = shoppingCart.ShoppingCartID,
                    CourseID = courseId,
                    Course = course,
                    AddedAt = DateTime.UtcNow
                };

                shoppingCart.Items.Add(newCartItem);
                await _context.SaveChangesAsync();

                _logger.LogWarning("Failed to add course {CourseId} to wishlist", courseId);
                return false;
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                _logger.LogError(ex, "Error moving course {CourseId} to wishlist for {Email}", courseId, email);
                throw;
            }
        }

        /// <summary>
        /// Removes all courses from student's shopping cart
        /// </summary>
        /// <param name="email">Student email</param>
        /// <returns>True if cleared successfully, false otherwise</returns>
        public async Task<bool> ClearShoppingCartAsync(string email)
        {
            try
            {
                _logger.LogInformation("Clearing shopping cart for email: {Email}", email);

                var shoppingCart = await GetOrCreateShoppingCartAsync(email);

                var itemsToRemove = shoppingCart.Items.ToList();
                foreach (var item in itemsToRemove)
                {
                    _context.ShoppingCartItem.Remove(item);
                }

                shoppingCart.Items.Clear();
                shoppingCart.LastModified = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Shopping cart cleared successfully for email: {Email}", email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing shopping cart for {Email}", email);
                throw;
            }
        }

        /// <summary>
        /// Gets shopping cart by ID with course details
        /// </summary>
        /// <param name="cartId">Shopping cart ID</param>
        /// <returns>Shopping cart with courses or null if not found</returns>
        public async Task<ShoppingCart?> GetShoppingCartByIdAsync(Guid cartId)
        {
            try
            {
                _logger.LogInformation("Getting shopping cart by ID: {CartId}", cartId);

                return await _context.ShoppingCart
                    .Include(sc => sc.Items)
                        .ThenInclude(item => item.Course)
                        .ThenInclude(c => c.CourseDetails)
                    .FirstOrDefaultAsync(sc => sc.ShoppingCartID == cartId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving shopping cart with ID {CartId}", cartId);
                throw;
            }
        }

        /// <summary>
        /// Gets number of items in student's shopping cart
        /// </summary>
        /// <param name="email">Student email</param>
        /// <returns>Number of items in cart</returns>
        public async Task<int> GetItemCountAsync(string email)
        {
            try
            {
                _logger.LogInformation("Getting item count for email: {Email}", email);

                var shoppingCart = await GetShoppingCartForStudentAsync(email);
                var count = shoppingCart?.Items.Count ?? 0;

                _logger.LogInformation("Shopping cart for {Email} has {Count} items", email, count);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting item count for {Email}", email);
                throw;
            }
        }

        /// <summary>
        /// Checks if a specific course is in the student's shopping cart
        /// </summary>
        /// <param name="email">Student email</param>
        /// <param name="courseId">Course ID to check</param>
        /// <returns>True if course is in cart, false otherwise</returns>
        public async Task<bool> IsCourseInCartAsync(string email, Guid courseId)
        {
            try
            {
                _logger.LogInformation("Checking if course {CourseId} is in cart for {Email}", courseId, email);

                var shoppingCart = await GetShoppingCartForStudentAsync(email);
                if (shoppingCart == null)
                {
                    return false;
                }

                var isInCart = shoppingCart.Items.Any(item => item.CourseID == courseId);
                _logger.LogInformation("Course {CourseId} is{NotInCart} in cart for {Email}",
                    courseId, isInCart ? "" : " not", email);

                return isInCart;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if course {CourseId} is in cart for {Email}", courseId, email);
                throw;
            }
        }
    }
}