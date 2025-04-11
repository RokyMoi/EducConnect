using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;
using EduConnect.Services;
using EduConnect.Interfaces.Shopping;
using EduConnect.Helpers;


[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IStudentEnrollmentService _enrollmentService;
    private readonly IShoppingCartService _shoppingCartService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(
        IConfiguration configuration,
        IStudentEnrollmentService enrollmentService,
        IShoppingCartService shoppingCartService,
        ILogger<PaymentController> logger)
    {
        _configuration = configuration;
        _enrollmentService = enrollmentService;
        _shoppingCartService = shoppingCartService;
        _logger = logger;

        // Set Stripe API Key
        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
    }

    // DTO for checkout request
    public class CheckoutRequest
    {
        public Guid? CartId { get; set; }
    }

    [HttpPost("create-checkout-session")]
    [Authorize]
    public async Task<IActionResult> CreateCheckoutSession([FromBody] CheckoutRequest request)
    {
        try
        {
            // Use Caller to get email
            var caller = new Caller(HttpContext);
            var callerEmail = caller.Email?.ToLower();

            if (string.IsNullOrEmpty(callerEmail))
            {
                return BadRequest("Caller email not provided");
            }

            // Retrieve shopping cart based on CartId or caller's email
            var cart = request.CartId.HasValue
                ? await _shoppingCartService.GetShoppingCartByIdAsync(request.CartId.Value)
                : await _shoppingCartService.GetShoppingCartForStudentAsync(callerEmail);

            if (cart == null || cart.Items == null || !cart.Items.Any())
            {
                return BadRequest("No courses found for checkout");
            }

            // Prepare line items for Stripe using course details
            var lineItems = cart.Items.Select(course => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = course.Title,
                        Description = course.CourseDetails.CourseDescription
                    },
                    UnitAmount = (long)(course.CourseDetails.Price * 100) // convert dollars to cents
                },
                Quantity = 1
            }).ToList();

            // Create session options for Stripe Checkout
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = $"{_configuration["AppSettings:FrontendUrl"]}/payment-success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_configuration["AppSettings:FrontendUrl"]}/shopping-cart",
                Metadata = new Dictionary<string, string>
                {
                    { "StudentEmail", callerEmail },
                    { "CartId", cart.ShoppingCartID.ToString() }
                }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Ok(new { sessionId = session.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Stripe checkout session");
            return StatusCode(500, new { message = "An error occurred while processing your payment", details = ex.Message });
        }
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> HandleStripeWebhook()
    {
        try
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"];

            var stripeEvent = EventUtility.ConstructEvent(
                json,
                stripeSignature,
                _configuration["Stripe:WebhookSecret"]
            );
            switch (stripeEvent.Type)
            {
                case "checkout.session.completed":
                    await HandleCheckoutSessionCompleted(stripeEvent);
                    break;
                case "payment_intent.payment_failed":
                    await HandlePaymentFailed(stripeEvent);
                    break;
                default:
                    _logger.LogInformation($"Unhandled event type: {stripeEvent.Type}");
                    break;
            }
            return Ok();
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe webhook processing error");
            return BadRequest(new { message = "Webhook processing failed", details = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected webhook error");
            return StatusCode(500, new { message = "Unexpected error", details = ex.Message });
        }
    }

    private async Task HandleCheckoutSessionCompleted(global::Stripe.Event stripeEvent)
    {
        var session = stripeEvent.Data.Object as Session;
        if (session == null) return;

        try
        {
            // Extract metadata: StudentEmail and CartId are expected in metadata
            if (!session.Metadata.TryGetValue("StudentEmail", out var studentEmail))
                throw new ArgumentException("Student email not found in metadata");

            if (!session.Metadata.TryGetValue("CartId", out var cartIdStr) ||
                !Guid.TryParse(cartIdStr, out var cartId))
                throw new ArgumentException("CartId not found or invalid in metadata");

            // Process enrollment based on the shopping cart
            await _enrollmentService.ProcessCourseEnrollmentFromCartAsync(studentEmail, cartId);

           // We clear then shopping cart
            await _shoppingCartService.ClearShoppingCartAsync(studentEmail);

            _logger.LogInformation($"Successfully processed checkout for {studentEmail}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing completed checkout session");
            
        }
    }

    private async Task HandlePaymentFailed(Stripe.Event stripeEvent)
    {
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        if (paymentIntent == null) return;

        _logger.LogWarning($"Payment failed: {paymentIntent.Id}, Reason: {paymentIntent.LastPaymentError?.Message}");
    }
}
