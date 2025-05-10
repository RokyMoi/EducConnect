using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using EduConnect.Entities.Shopping;
using EduConnect.Interfaces.Shopping;
using EduConnect.Services;
using EduConnect.Extensions;
using backend.Middleware;
using EduConnect.Helpers;
using Stripe.V2;
using EduConnect.Entities.Course;
using Microsoft.EntityFrameworkCore;
using EduConnect.Data;

namespace EduConnect.Controllers
{
    /// <summary>
    /// Exposes endpoints for creating Stripe Checkout sessions,
    /// handling webhooks and querying payment status.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    
    public class PaymentController : ControllerBase
    {
        private readonly StripeSettings _stripeSettings;
        private readonly string _frontendUrl;
        private readonly IStudentEnrollmentService _enrollmentService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ILogger<PaymentController> _logger;
        private readonly DataContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentController"/> class.
        /// </summary>
        public PaymentController(
            IOptions<StripeSettings> stripeOptions,
            IConfiguration configuration,
            IStudentEnrollmentService enrollmentService,
            IShoppingCartService shoppingCartService,
            ILogger<PaymentController> logger,DataContext context)
        {
            _stripeSettings = stripeOptions.Value ?? throw new ArgumentNullException(nameof(stripeOptions));
            _frontendUrl = configuration["AppSettings:FrontendUrl"]
                                    ?? throw new ArgumentException("FrontendUrl not configured");
            _enrollmentService = enrollmentService ?? throw new ArgumentNullException(nameof(enrollmentService));
            _shoppingCartService = shoppingCartService ?? throw new ArgumentNullException(nameof(shoppingCartService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        /// <summary>
        /// Request DTO for initiating checkout.
        /// </summary>
        public class CheckoutRequest
        {
            public Guid? CartId { get; set; }
        }

        /// <summary>
        /// Creates a Stripe Checkout session, collects email/card/billing address.
        /// </summary>
        /// <param name="request">Contains optional CartId to process.</param>
        /// <returns>Session ID and publishable key on success.</returns>
        
        [HttpPost("create-checkout-session")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CheckoutRequest request)
        {
            // 1. Validate caller
            var callerEmail = new Caller(HttpContext).Email?.ToLowerInvariant();
            if (string.IsNullOrEmpty(callerEmail))
            {
                _logger.LogWarning("Caller email not provided");
                return BadRequest("Caller email not provided");
            }
            _logger.LogInformation("Initiating checkout for {Email}", callerEmail);

            // 2. Load shopping cart (ensure CourseDetails is eager-loaded in service)
            var cart = request.CartId.HasValue
                ? await _shoppingCartService.GetShoppingCartByIdAsync(request.CartId.Value)
                : await _shoppingCartService.GetShoppingCartForStudentAsync(callerEmail);

            if (cart?.Items == null || !cart.Items.Any())
            {
                _logger.LogWarning("No courses found in cart for {Email}", callerEmail);
                return BadRequest("No courses found for checkout");
            }

            // 3. Build Stripe line items
            var lineItems = new List<SessionLineItemOptions>();
            foreach (var item in cart.Items)
            {
                var course = item.Course;
                var details = course.CourseDetails;
                decimal unitPrice = 0m;

                if (details != null && details.Price > 0)
                {
                    unitPrice = (decimal)details.Price;
                    _logger.LogDebug("Using CourseDetails.Price {Price} for {Course}", unitPrice, course.Title);
                }
                else if (course.Price > 0)
                {
                    unitPrice = course.Price;
                    _logger.LogDebug("Falling back to Course.Price {Price} for {Course}", unitPrice, course.Title);
                }
                else
                {
                    _logger.LogWarning("Skipping {Course} – price invalid or not set", course.Title);
                    continue;
                }

                long amountInCents = (long)(unitPrice * 100);
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = amountInCents,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = course.Title,
                            Description = TruncateDescription(details?.CourseDescription ?? course.Description)
                        }
                    },
                    Quantity = 1
                });
            }

            if (!lineItems.Any())
            {
                _logger.LogWarning("No valid line items generated for {Email}", callerEmail);
                return BadRequest("No valid items to process.");
            }

            // 4. Prepare Stripe session options
            var sessionOptions = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                CustomerCreation = "always",            // force customer and form
                BillingAddressCollection = "required",          // require billing address
                SuccessUrl = $"{_frontendUrl}/payment-success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_frontendUrl}/cart-dwe",
                Metadata = new Dictionary<string, string>
                {
                    ["StudentEmail"] = callerEmail,
                    ["CartId"] = cart.ShoppingCartID.ToString(),
                    ["TotalItems"] = cart.Items.Count.ToString()
                },
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    CaptureMethod = "automatic"
                },
                Locale = "auto"
            };

            // 5. Create session
            var session = await new SessionService().CreateAsync(sessionOptions);
            _logger.LogInformation("Created Stripe session {SessionId} for {Email}", session.Id, callerEmail);

            return Ok(new
            {
                sessionId = session.Id,
                publishableKey = _stripeSettings.PublishableKey
            });
        }

        /// <summary>
        /// Stripe webhook endpoint for handling payment events.
        /// </summary>
        [HttpPost("webhook")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> HandleStripeWebhook()
        {
            // 1. Učitaj telo zahteva i Stripe potpis
            string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            string sig = Request.Headers["Stripe-Signature"];
            if (string.IsNullOrEmpty(sig))
                return BadRequest("Missing Stripe signature");

            Stripe.Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, sig, _stripeSettings.WebhookSecret);
                _logger.LogInformation("Received Stripe event type: {Type}", stripeEvent.Type);
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Invalid Stripe webhook signature");
                return BadRequest("Webhook signature verification failed");
            }

            // 2. Dispatch prema konkretnim tipovima eventa
            switch (stripeEvent.Type)
            {
                case "checkout.session.completed":
                    await OnCheckoutSessionCompleted(stripeEvent);
                    break;

                case "checkout.session.async_payment_succeeded":
                    // Ovaj event dolazi kad se asinkrono plaćanje uspešno završi
                    await OnCheckoutSessionCompleted(stripeEvent);
                    break;

                case "checkout.session.async_payment_failed":
                    // Asinkrono plaćanje nije uspelo
                    _logger.LogWarning(
                        "Async payment failed for session {SessionId}",
                        ((Session)stripeEvent.Data.Object)?.Id
                    );
                    break;

                case "payment_intent.succeeded":
                    _logger.LogInformation(
                        "PaymentIntent succeeded {PaymentIntentId}",
                        ((PaymentIntent)stripeEvent.Data.Object)?.Id
                    );
                    break;

                case "payment_intent.payment_failed":
                    _logger.LogWarning(
                        "PaymentIntent failed {PaymentIntentId} – {Error}",
                        ((PaymentIntent)stripeEvent.Data.Object)?.Id,
                        ((PaymentIntent)stripeEvent.Data.Object)?.LastPaymentError?.Message
                    );
                    break;

                default:
                    _logger.LogInformation("Unhandled Stripe event type: {Type}", stripeEvent.Type);
                    break;
            }

            return Ok();
        }



        /// <summary>
        /// Retrieves payment status for a given Stripe session.
        /// </summary>
        [HttpGet("payment-status/{sessionId}")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> CheckPaymentStatus(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                return BadRequest("Session ID is required");

            try
            {
                var session = await new SessionService().GetAsync(sessionId);
                if (session is null)
                    return NotFound("Session not found");

                return Ok(new
                {
                    session.PaymentStatus,
                    session.CustomerEmail,
                    AmountTotal = session.AmountTotal / 100.0m,
                    session.Currency
                });
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Error retrieving session {SessionId}", sessionId);
                return StatusCode(500, "Error checking payment status");
            }
        }

        /// <summary>
        /// Processes a completed Stripe checkout session, enrolling the student in purchased courses
        /// and clearing their shopping cart.
        /// </summary>
        /// <param name="stripeEvent">The Stripe webhook event containing the session data</param>
        /// <returns>A task representing the asynchronous operation</returns>
        private async Task OnCheckoutSessionCompleted(Stripe.Event stripeEvent)
        {
            Console.WriteLine($"[STRIPE_WEBHOOK] Starting to process checkout session completed event: {stripeEvent?.Id}");

            // Validate that the event contains a valid session object
            if (stripeEvent?.Data?.Object is not Session session)
            {
                _logger.LogWarning("Stripe event does not contain a valid Session object. EventId: {EventId}", stripeEvent?.Id);
                Console.WriteLine($"[STRIPE_WEBHOOK] ERROR: Invalid session object in event {stripeEvent?.Id}");
                return;
            }

            Console.WriteLine($"[STRIPE_WEBHOOK] Session found with ID: {session.Id}");

            // Get the user's email from session metadata instead of HTTP context
            string email;
            if (session.Metadata.TryGetValue("StudentEmail", out var studentEmail) && !string.IsNullOrWhiteSpace(studentEmail))
            {
                email = studentEmail.ToLowerInvariant();
                Console.WriteLine($"[STRIPE_WEBHOOK] Found student email in session metadata: {email}");
            }
            else if (!string.IsNullOrWhiteSpace(session.CustomerEmail))
            {
                email = session.CustomerEmail.ToLowerInvariant();
                Console.WriteLine($"[STRIPE_WEBHOOK] Using customer email from session: {email}");
            }
            else
            {
                _logger.LogError("No valid email found in session metadata or customer data. Session ID: {SessionId}", session.Id);
                Console.WriteLine($"[STRIPE_WEBHOOK] ERROR: No valid email found for session {session.Id}");
                return;
            }

            Console.WriteLine($"[STRIPE_WEBHOOK] Processing payment for user: {email}");

            // Extract and validate the shopping cart ID from session metadata
            if (!session.Metadata.TryGetValue("CartId", out var cartIdString) || !Guid.TryParse(cartIdString, out var cartId))
            {
                _logger.LogWarning("Cart ID missing or invalid in session metadata. Session ID: {SessionId}", session.Id);
                Console.WriteLine($"[STRIPE_WEBHOOK] ERROR: Invalid or missing cart ID in session metadata: {session.Id}");
                return;
            }

            Console.WriteLine($"[STRIPE_WEBHOOK] Cart ID extracted: {cartId}");
            _logger.LogInformation("Processing completed checkout session for user {Email}, cart ID: {CartId}", email, cartId);

            try
            {
                // Step 1: Retrieve the person entity associated with the email
                Console.WriteLine($"[STRIPE_WEBHOOK] Searching for person with email: {email}");
                var person = await _context.PersonEmail
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Email == email);
                if (person == null)
                {
                    _logger.LogWarning("Person not found for email: {Email}", email);
                    Console.WriteLine($"[STRIPE_WEBHOOK] ERROR: No person record found for email: {email}");
                    return;
                }

                Console.WriteLine($"[STRIPE_WEBHOOK] Person found with ID: {person.PersonId}");

                // Step 2: Find the student record for this person
                Console.WriteLine($"[STRIPE_WEBHOOK] Searching for student record with person ID: {person.PersonId}");
                var student = await _context.Student
                    .FirstOrDefaultAsync(x => x.PersonId == person.PersonId);
                if (student == null)
                {
                    _logger.LogWarning("Student not found for personId: {PersonId}", person.PersonId);
                    Console.WriteLine($"[STRIPE_WEBHOOK] ERROR: No student record found for person ID: {person.PersonId}");
                    return;
                }

                Console.WriteLine($"[STRIPE_WEBHOOK] Student found with ID: {student.StudentId}");

                // Step 3: Retrieve all items from the shopping cart
                Console.WriteLine($"[STRIPE_WEBHOOK] Retrieving items from cart ID: {cartId}");
                var cartItems = await _context.ShoppingCartItems
                    .Where(i => i.ShoppingCartID == cartId)
                    .ToListAsync();
                if (!cartItems.Any())
                {
                    _logger.LogWarning("No items found in shopping cart {CartId}", cartId);
                    Console.WriteLine($"[STRIPE_WEBHOOK] ERROR: Shopping cart {cartId} is empty");
                    return;
                }

                Console.WriteLine($"[STRIPE_WEBHOOK] Found {cartItems.Count} items in shopping cart");

                // Step 4: Process each cart item and create enrollments
                int enrollmentsCreated = 0;
                int itemsProcessed = 0;
                Console.WriteLine($"[STRIPE_WEBHOOK] Starting to process cart items and create enrollments");

                foreach (var item in cartItems)
                {
                    itemsProcessed++;
                    Console.WriteLine($"[STRIPE_WEBHOOK] Processing cart item {itemsProcessed}/{cartItems.Count}, Course ID: {item.CourseID}");

                    // Find the course associated with this cart item
                    var course = await _context.Course
                        .FirstOrDefaultAsync(c => c.CourseId == item.CourseID);
                    if (course == null)
                    {
                        _logger.LogWarning("Course not found for ID: {CourseId}", item.CourseID);
                        Console.WriteLine($"[STRIPE_WEBHOOK] WARNING: Course not found with ID: {item.CourseID}, skipping enrollment");
                        continue;
                    }

                    Console.WriteLine($"[STRIPE_WEBHOOK] Course found: '{course.Title}' (ID: {course.CourseId})");

                    // Check if the student is already enrolled in this course
                    var alreadyEnrolled = await _context.StudentEnrollment
                        .AnyAsync(e => e.CourseId == course.CourseId && e.StudentId == student.StudentId);
                    if (alreadyEnrolled)
                    {
                        _logger.LogInformation("Student {StudentId} already enrolled in course {CourseId}", student.StudentId, course.CourseId);
                        Console.WriteLine($"[STRIPE_WEBHOOK] Student already enrolled in course '{course.Title}', skipping");
                        continue;
                    }

                    // Create a new enrollment record
                    Console.WriteLine($"[STRIPE_WEBHOOK] Creating new enrollment for student {student.StudentId} in course {course.CourseId}");
                    var enrollment = new StudentEnrollment
                    {
                        StudentEnrollmentId = Guid.NewGuid(),
                        StudentId = student.StudentId,
                        CourseId = course.CourseId,
                        EnrollmentDate = DateTime.UtcNow,
                        Status = EnrollmentStatus.Active
                    };
                    await _context.StudentEnrollment.AddAsync(enrollment);
                    enrollmentsCreated++;
                    Console.WriteLine($"[STRIPE_WEBHOOK] Enrollment {enrollment.StudentEnrollmentId} created successfully");
                }

                // Step 5: Save all enrollments to the database
                Console.WriteLine($"[STRIPE_WEBHOOK] Saving {enrollmentsCreated} new enrollments to database");
                await _context.SaveChangesAsync();
                _logger.LogInformation("Student {StudentId} successfully enrolled in {Count} courses", student.StudentId, enrollmentsCreated);
                Console.WriteLine($"[STRIPE_WEBHOOK] Successfully saved all enrollments to database");

                // Step 6: Clear the user's shopping cart after successful enrollment
                Console.WriteLine($"[STRIPE_WEBHOOK] Clearing shopping cart for user {email}");
                await _shoppingCartService.ClearShoppingCartAsync(email);
                _logger.LogInformation("Shopping cart cleared for user {Email} after successful checkout", email);
                Console.WriteLine($"[STRIPE_WEBHOOK] Shopping cart successfully cleared");

                Console.WriteLine($"[STRIPE_WEBHOOK] Checkout session processing completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing checkout session. Cart ID: {CartId}, Session ID: {SessionId}", cartId, session.Id);
                Console.WriteLine($"[STRIPE_WEBHOOK] ERROR: Exception occurred during checkout processing: {ex.Message}");
                Console.WriteLine($"[STRIPE_WEBHOOK] ERROR: Stack trace: {ex.StackTrace}");
                // Note: We don't rethrow the exception as this is likely called from a webhook handler
                // where we want to return a 200 OK to Stripe regardless of our internal processing
            }
        }

        /// <summary>
        /// Truncates long descriptions to Stripe’s limit (500 chars).
        /// </summary>
        private static string TruncateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return "Course by EduConnect";

            const int MaxLength = 500;
            return description.Length <= MaxLength
                ? description
                : description.Substring(0, MaxLength - 3) + "...";
        }


    }
}
