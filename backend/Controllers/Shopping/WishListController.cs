using EduConnect.Entities.Shopping;
using EduConnect.Interfaces.Shopping;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateWishlist([FromQuery] string email)
        {
            var wishlist = await _wishlistService.CreateWishlistAsync(email);
            return Ok(wishlist);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetWishlistForStudent(string email)
        {
            var wishlist = await _wishlistService.GetWishlistForStudentAsync(email);
            if (wishlist == null) return NotFound("Wishlist not found");
            return Ok(wishlist);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToWishlist([FromQuery] string email, [FromQuery] Guid courseId)
        {
            var result = await _wishlistService.AddCourseToWishlistAsync(email, courseId);
            return result ? Ok("Course added to wishlist") : BadRequest("Course already in wishlist");
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFromWishlist([FromQuery] string email, [FromQuery] Guid courseId)
        {
            var result = await _wishlistService.RemoveCourseFromWishlistAsync(email, courseId);
            return result ? Ok("Course removed from wishlist") : NotFound("Course not found in wishlist");
        }

        [HttpPost("move-to-cart")]
        public async Task<IActionResult> MoveToShoppingCart([FromQuery] string email, [FromQuery] Guid courseId)
        {
            var result = await _wishlistService.MoveCourseToShoppingCartAsync(email, courseId);
            return result ? Ok("Moved to shopping cart") : BadRequest("Failed to move to cart");
        }
    }
}
