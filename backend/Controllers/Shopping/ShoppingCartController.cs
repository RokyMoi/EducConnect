using backend.Middleware;
using EduConnect.Data;
using EduConnect.Entities.Shopping;
using EduConnect.Helpers;
using EduConnect.Interfaces.Shopping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EduConnect.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [CheckPersonLoginSignup]

    public class ShoppingCartController(IShoppingCartService _shoppingCartService, ILogger<ShoppingCartController> _logger, DataContext _context) : ControllerBase
    {
        

        /// <summary>
        /// Gets the current user's email from their claims
        /// </summary>
        private string GetCurrentUserEmail()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                throw new UnauthorizedAccessException("User email not found in token");
            }
            return email;
        }

        /// <summary>
        /// Get the current user's shopping cart
        /// </summary>
        /// <returns>Shopping cart with all courses</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ShoppingCart), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetShoppingCart()
        {
            try
            {
                var email = GetCurrentUserEmail();
                _logger.LogInformation("Getting shopping cart for user: {Email}", email);

                var shoppingCart = await _shoppingCartService.GetShoppingCartForStudentAsync(email);
                if (shoppingCart == null)
                {
                    return NotFound("Shopping cart not found");
                }

                return Ok(shoppingCart);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to shopping cart");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving shopping cart");
                return StatusCode(500, "An error occurred while retrieving the shopping cart");
            }
        }

        /// <summary>
        /// Creates a new shopping cart for the user
        /// </summary>
        /// <returns>Newly created shopping cart</returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ShoppingCart), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateShoppingCart()
        {
            try
            {
                var email = GetCurrentUserEmail();
                _logger.LogInformation("Creating shopping cart for user: {Email}", email);

                var shoppingCart = await _shoppingCartService.CreateShoppingCartAsync(email);
                return Ok(shoppingCart);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to create shopping cart");
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument while creating shopping cart");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating shopping cart");
                return StatusCode(500, "An error occurred while creating the shopping cart");
            }
        }

        /// <summary>
        /// Add a course to the shopping cart
        /// </summary>
        /// <param name="courseId">ID of the course to add</param>
        /// <returns>Success status</returns>
        [HttpPost("add/{courseId}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddCourseToCart(Guid courseId)
        {
            try
            {
                var email = GetCurrentUserEmail();
                _logger.LogInformation("Adding course {CourseId} to cart for user: {Email}", courseId, email);

                var result = await _shoppingCartService.AddCourseToShoppingCartAsync(email, courseId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to add course to cart");
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument while adding course to cart: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding course {CourseId} to cart", courseId);
                return StatusCode(500, "An error occurred while adding the course to the cart");
            }
        }

        /// <summary>
        /// Remove a course from the shopping cart
        /// </summary>
        /// <param name="courseId">ID of the course to remove</param>
        /// <returns>Success status</returns>
        [HttpDelete("remove/{courseId}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RemoveCourseFromCart(Guid courseId)
        {
            try
            {
                var email = GetCurrentUserEmail();
                _logger.LogInformation("Removing course {CourseId} from cart for user: {Email}", courseId, email);

                var result = await _shoppingCartService.DeleteShoppingCartItemAsync(email, courseId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to remove course from cart");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing course {CourseId} from cart", courseId);
                return StatusCode(500, "An error occurred while removing the course from the cart");
            }
        }

        /// <summary>
        /// Move a course from shopping cart to wishlist
        /// </summary>
        /// <param name="courseId">ID of the course to move</param>
        /// <returns>Success status</returns>
        [HttpPost("move-to-wishlist/{courseId}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> MoveCourseToWishlist(Guid courseId)
        {
            try
            {
                var email = GetCurrentUserEmail();
                _logger.LogInformation("Moving course {CourseId} to wishlist for user: {Email}", courseId, email);

                var result = await _shoppingCartService.MoveCourseToWishListAsync(email, courseId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to move course to wishlist");
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument while moving course to wishlist: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error moving course {CourseId} to wishlist", courseId);
                return StatusCode(500, "An error occurred while moving the course to the wishlist");
            }
        }

        /// <summary>
        /// Clear all items from the shopping cart
        /// </summary>
        /// <returns>Success status</returns>
        [HttpDelete("clear")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ClearShoppingCart()
        {
            try
            {
                var email = GetCurrentUserEmail();
                _logger.LogInformation("Clearing shopping cart for user: {Email}", email);

                var result = await _shoppingCartService.ClearShoppingCartAsync(email);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to clear shopping cart");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing shopping cart");
                return StatusCode(500, "An error occurred while clearing the shopping cart");
            }
        }

        /// <summary>
        /// Get the total price of all items in the cart
        /// </summary>
        /// <returns>Total price as decimal</returns>
        [HttpGet("total-price")]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTotalPrice()
        {
            try
            {
                var email = GetCurrentUserEmail();
                _logger.LogInformation("Getting total price for user: {Email}", email);

                var cart = await _shoppingCartService.GetShoppingCartForStudentAsync(email);
                if (cart == null)
                {
                    return NotFound("Shopping cart not found");
                }

                var totalPrice = await _shoppingCartService.GetTotalPriceAsync(cart.ShoppingCartID);
                return Ok(totalPrice);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to get total price");
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument while getting total price: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total price");
                return StatusCode(500, "An error occurred while calculating the total price");
            }
        }

        /// <summary>
        /// Get the number of items in the shopping cart
        /// </summary>
        /// <returns>Item count</returns>
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> GetItemCount()
        {
            try
            {
                var email = GetCurrentUserEmail();
                _logger.LogInformation("Getting item count for user: {Email}", email);

                var count = await _shoppingCartService.GetItemCountAsync(email);
                return Ok(count);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to get item count");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting item count");
                return StatusCode(500, "An error occurred while getting the item count");
            }
        }

        /// <summary>
        /// Check if a course is in the shopping cart
        /// </summary>
        /// <param name="courseId">ID of the course to check</param>
        /// <returns>True if course is in cart, false otherwise</returns>
        [HttpGet("contains/{courseId}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> CheckIfCourseInCart(Guid courseId)
        {
            try
            {
                var email = GetCurrentUserEmail();
                _logger.LogInformation("Checking if course {CourseId} is in cart for user: {Email}", courseId, email);

                var isInCart = await _shoppingCartService.IsCourseInCartAsync(email, courseId);
                return Ok(isInCart);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt to check if course in cart");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if course {CourseId} is in cart", courseId);
                return StatusCode(500, "An error occurred while checking if the course is in the cart");
            }
        }
        [HttpGet]
        [Route("/loadShopping")]
        public async Task<IActionResult> LoadShoppingCartCourses()
        {
            try
            {
                var caller = new Caller(this.HttpContext);
                var email = caller.Email;

                // Find the student ID via email
                var student = await _context.Student
                    .Include(s => s.Person)
                    .ThenInclude(p => p.PersonEmail)
                    .FirstOrDefaultAsync(s => s.Person.PersonEmail.Email == email);

                if (student == null)
                {
                    return NotFound("Student not found.");
                }

                // Get wishlist with course details and thumbnails
                var shoppingcart = await _context.ShoppingCart
                    .Include(w => w.Items)
                        .ThenInclude(i => i.Course)
                            .ThenInclude(c => c.CourseThumbnail)
                    .FirstOrDefaultAsync(w => w.StudentID == student.StudentId);

                if (shoppingcart == null || shoppingcart.Items.Count == 0)
                {
                    return Ok(new List<object>()); // return empty list
                }

                var result = shoppingcart.Items.Select(item => new
                {
                    CourseId = item.CourseID,
                    Title = item.Course.Title,
                    Price = item.Course.Price,
                    ThumbnailUrl = item.Course.CourseThumbnail?.ThumbnailUrl
                });

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wishlist course information.");
                return StatusCode(500, "An error occurred while retrieving wishlist course information.");
            }
        }
    }
}

