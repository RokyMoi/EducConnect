using EduConnect.Entities.Shopping;
using EduConnect.Interfaces.Shopping;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Controllers.Shopping
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController(IPaymentService payment):MainController
    {
        [HttpPost("AddPaymment")]
        public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePaymentIntent(Guid cartid)
        {
            var cart = payment.CreateOrUpdatePaymentIntent(cartid);

            if(cart == null)
            {
                return BadRequest("Invalid cart, cant be found");
            }
            return Ok(cart);
        }

    }
}
