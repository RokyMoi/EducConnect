using EduConnect.Helpers;
using EduConnect.Interfaces.Shopping;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Controllers.Shopping
{
    public class ShoppingCartController(IShoppingCartService _shoppingCartService):MainController
    {
       

    
        private string GetCallerEmail()
        {
            var caller = new Caller(HttpContext);
            return caller.Email;
        }

        [HttpPost("createShoppingCartForUser")]
        public async Task<IActionResult> CreateShoppingCart()
        {
            try
            {
                var email = GetCallerEmail();
                var shoppingCart = await _shoppingCartService.CreateShoppingCartAsync(email);
                return Ok(shoppingCart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("delete-item/{courseID}")]
        public async Task<IActionResult> DeleteShoppingCartItem(Guid courseID)
        {
            try
            {
                var email = GetCallerEmail();
                var success = await _shoppingCartService.DeleteShoppingCartItemAsync(email, courseID);

                if (!success)
                {
                    return NotFound(new { message = "Course not found in the shopping cart" });
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("getShoppingCartForUser")]
        public async Task<IActionResult> GetShoppingCart()
        {
            try
            {
                var email = GetCallerEmail();
                var shoppingCart = await _shoppingCartService.GetShoppingCartForStudentAsync(email);

                if (shoppingCart == null)
                {
                    return NotFound(new { message = "Shopping cart not found" });
                }

                return Ok(shoppingCart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("add-shopping-item/{courseID}")]
        public async Task<IActionResult> AddItemToShoppingCart(Guid courseID)
        {
            try
            {
                var email = GetCallerEmail();
                var success = await _shoppingCartService.SetShoppingCartAsync(email, courseID);

                if (!success)
                {
                    return NotFound(new { message = "Shopping cart not found" });
                }

                return Ok(new { message = "Course added to the shopping cart" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
    

