using EduConnect.Entities.Shopping;
using EduConnect.Interfaces.Shopping;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCart([FromQuery] string email)
        {
            var cart = await _shoppingCartService.CreateShoppingCartAsync(email);
            return Ok(cart);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetCartForStudent(string email)
        {
            var cart = await _shoppingCartService.GetShoppingCartForStudentAsync(email);
            if (cart == null) return NotFound("Shopping cart not found");
            return Ok(cart);
        }

        [HttpGet("by-id/{cartId}")]
        public async Task<IActionResult> GetCartById(Guid cartId)
        {
            var cart = await _shoppingCartService.GetShoppingCartByIdAsync(cartId);
            if (cart == null) return NotFound("Shopping cart not found");
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCourseToCart([FromQuery] string email, [FromQuery] Guid courseId)
        {
            var result = await _shoppingCartService.AddCourseToShoppingCartAsync(email, courseId);
            return result ? Ok("Course added to cart") : BadRequest("Course already in cart");
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveCourseFromCart([FromQuery] string email, [FromQuery] Guid courseId)
        {
            var result = await _shoppingCartService.DeleteShoppingCartItemAsync(email, courseId);
            return result ? Ok("Course removed from cart") : NotFound("Course not found in cart");
        }

        [HttpPost("move-to-wishlist")]
        public async Task<IActionResult> MoveToWishlist([FromQuery] string email, [FromQuery] Guid courseId)
        {
            var result = await _shoppingCartService.MoveCourseToWishListAsync(email, courseId);
            return result ? Ok("Moved to wishlist") : BadRequest("Failed to move to wishlist");
        }

        [HttpPost("clear")]
        public async Task<IActionResult> ClearCart([FromQuery] string email)
        {
            var result = await _shoppingCartService.ClearShoppingCartAsync(email);
            return result ? Ok("Cart cleared") : NotFound("Shopping cart not found");
        }

        [HttpGet("total-price")]
        public async Task<IActionResult> GetTotalPrice([FromQuery] Guid cartId)
        {
            var total = await _shoppingCartService.GetTotalPriceAsync(cartId);
            return Ok(total);
        }

        [HttpGet("item-count")]
        public async Task<IActionResult> GetItemCount([FromQuery] string email)
        {
            var count = await _shoppingCartService.GetItemCountAsync(email);
            return Ok(count);
        }
    }
}
