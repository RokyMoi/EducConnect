using EduConnect.Data;
using EduConnect.Interfaces.Shopping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Middleware;
using EduConnect.Helpers;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [CheckPersonLoginSignup]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;
        private readonly ILogger<WishlistController> _logger;
        private readonly DataContext _context;

        // Jedan konstruktor koji prima sve zavisnosti putem DI
        public WishlistController(IWishlistService wishlistService, ILogger<WishlistController> logger, DataContext context)
        {
            _wishlistService = wishlistService ?? throw new ArgumentNullException(nameof(wishlistService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private string GetUserEmail()
        {
            var email = new Caller(this.HttpContext).Email;
            if (string.IsNullOrEmpty(email))
                throw new UnauthorizedAccessException("User email not found in token");
            return email;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            try
            {
                var email = GetUserEmail();
                var wishlist = await _wishlistService.GetWishlistForStudentAsync(email);

                if (wishlist == null)
                    return NotFound(new { message = "Wishlist not found" });

                return Ok(wishlist);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access to wishlist");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wishlist");
                return StatusCode(500, new { message = "An error occurred while retrieving the wishlist" });
            }
        }

        [HttpPost("courses/{courseId}")]
        public async Task<IActionResult> AddCourseToWishlist(string courseId)
        {
            try
            {
                if (string.IsNullOrEmpty(courseId))
                    return BadRequest(new { message = "Course ID cannot be null or empty" });

                if (!Guid.TryParse(courseId, out Guid parsedCourseId))
                    return BadRequest(new { message = $"Invalid course ID format: {courseId}" });

                var email = GetUserEmail();
                var added = await _wishlistService.AddCourseToWishlistAsync(email, parsedCourseId);

                return Ok(new
                {
                    message = added
                        ? "Course added to wishlist successfully"
                        : "Course is already in wishlist"
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request adding course {CourseId}", courseId);
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access adding course to wishlist");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding course {CourseId} to wishlist", courseId);
                return StatusCode(500, new { message = "An error occurred while adding the course to wishlist" });
            }
        }

        [HttpDelete("courses/{courseId}")]
        public async Task<IActionResult> RemoveCourseFromWishlist(string courseId)
        {
            try
            {
                if (string.IsNullOrEmpty(courseId))
                    return BadRequest(new { message = "Course ID cannot be null or empty" });

                if (!Guid.TryParse(courseId, out Guid parsedCourseId))
                    return BadRequest(new { message = $"Invalid course ID format: {courseId}" });

                var email = GetUserEmail();
                var removed = await _wishlistService.RemoveCourseFromWishlistAsync(email, parsedCourseId);

                return removed
                    ? Ok(new { message = "Course removed from wishlist successfully" })
                    : NotFound(new { message = "Course not found in wishlist" });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access removing course from wishlist");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing course {CourseId}", courseId);
                return StatusCode(500, new { message = "An error occurred while removing the course from wishlist" });
            }
        }

        [HttpGet("courses/{courseId}/exists")]
        public async Task<IActionResult> IsCourseInWishlist(string courseId)
        {
            try
            {
                if (string.IsNullOrEmpty(courseId))
                    return BadRequest(new { message = "Course ID cannot be null or empty" });

                if (!Guid.TryParse(courseId, out Guid parsedCourseId))
                    return BadRequest(new { message = $"Invalid course ID format: {courseId}" });

                var email = GetUserEmail();
                var exists = await _wishlistService.IsCourseInWishlistAsync(email, parsedCourseId);

                return Ok(new { exists });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access checking course in wishlist");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking course {CourseId}", courseId);
                return StatusCode(500, new { message = "An error occurred while checking the course in wishlist" });
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetWishlistItemCount()
        {
            try
            {
                var email = GetUserEmail();
                var count = await _wishlistService.GetItemCountAsync(email);
                return Ok(new { count });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access getting wishlist count");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting wishlist count");
                return StatusCode(500, new { message = "An error occurred while getting the wishlist count" });
            }
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearWishlist()
        {
            try
            {
                var email = GetUserEmail();
                await _wishlistService.ClearWishlistAsync(email);
                return Ok(new { message = "Wishlist cleared successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access clearing wishlist");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing wishlist");
                return StatusCode(500, new { message = "An error occurred while clearing the wishlist" });
            }
        }

        [HttpGet]
        [Route("/UcitajKurseve")]
        public async Task<IActionResult> GetWishlistCourses()
        {
            try
            {
                var email = GetUserEmail();

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
                var wishlist = await _context.Wishlist
                    .Include(w => w.Items)
                        .ThenInclude(i => i.Course)
                            .ThenInclude(c => c.CourseThumbnail)
                    .FirstOrDefaultAsync(w => w.StudentID == student.StudentId);

                if (wishlist == null || wishlist.Items.Count == 0)
                {
                    return Ok(new List<object>()); // return empty list
                }

                var result = wishlist.Items.Select(item => new
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
