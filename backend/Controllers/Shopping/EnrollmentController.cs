using backend.Middleware;
using EduConnect.Data;
using EduConnect.Entities.Course;
using EduConnect.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [CheckPersonLoginSignup]
    public class EnrollmentController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<EnrollmentController> _logger;

        public EnrollmentController(DataContext context, ILogger<EnrollmentController> logger)
        {
            _context = context;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/enrollment/{courseId}/enrollment-status
        [HttpGet("{courseId}/enrollment-status")]
        public async Task<IActionResult> GetEnrollmentStatus(Guid courseId)
        {
            var course = await _context.Course
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (course == null)
                return NotFound(new { message = "Course not found." });

            var buttonLabel = course.Price == 0 ? "Enroll" : "Add to Shopping Cart";

            return Ok(new
            {
                buttonLabel,
                price = course.Price
            });
        }

        // POST: api/enrollment/enroll
        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll([FromBody] CourseEnrollRequest request)
        {
            // 1. Log the incoming request for debugging
            _logger.LogInformation($"Received enrollment request: {System.Text.Json.JsonSerializer.Serialize(request)}");

            // 2. Check if request is null
            if (request == null)
            {
                _logger.LogWarning("Request body is null");
                return BadRequest(new { message = "Request body is null" });
            }

            // 3. Check if CourseId is provided
            if (request.CourseId == Guid.Empty)
            {
                _logger.LogWarning("CourseId is empty");
                return BadRequest(new { message = "CourseId is empty or missing" });
            }

            // 4. Parse the GUID (though this may be redundant if it's already a GUID)
            if (!Guid.TryParse(request.CourseId.ToString(), out Guid courseId))
            {
                _logger.LogWarning($"Invalid CourseId format: {request.CourseId}");
                return BadRequest(new { message = "Invalid course ID format." });
            }

            // 5. Check authentication
            var caller = new Caller(HttpContext);
            var userEmail = caller.Email;

            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.LogWarning("Authentication failed: Email not found in context");
                return Unauthorized(new { message = "Unauthorized. Email not found in context." });
            }

            try
            {
                // 6. Find the person
                var person = await _context.PersonEmail
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Email == userEmail);

                if (person == null)
                {
                    _logger.LogWarning($"Person not found for email: {userEmail}");
                    return NotFound(new { message = "Person with given email not found." });
                }

                // 7. Find the student
                var student = await _context.Student
                    .FirstOrDefaultAsync(x => x.PersonId == person.PersonId);

                if (student == null)
                {
                    _logger.LogWarning($"Student not found for PersonId: {person.PersonId}");
                    return NotFound(new { message = "Student not found for the given user." });
                }

                // 8. Find the course
                var course = await _context.Course
                    .FirstOrDefaultAsync(c => c.CourseId == courseId);

                if (course == null)
                {
                    _logger.LogWarning($"Course not found with ID: {courseId}");
                    return NotFound(new { message = "Course not found." });
                }

                // 9. Check if already enrolled
                var alreadyEnrolled = await _context.StudentEnrollment
                    .AnyAsync(e => e.CourseId == courseId && e.StudentId == student.StudentId);

                if (alreadyEnrolled)
                {
                    _logger.LogWarning($"Student {student.StudentId} already enrolled in course {courseId}");
                    return BadRequest(new { message = "Student already enrolled in this course." });
                }

                // 10. Create enrollment
                var enrollment = new StudentEnrollment
                {
                    StudentEnrollmentId = Guid.NewGuid(),
                    StudentId = student.StudentId,
                    CourseId = courseId,
                    EnrollmentDate = DateTime.UtcNow,
                    Status = EnrollmentStatus.Active
                };

                // 11. Save to database
                await _context.StudentEnrollment.AddAsync(enrollment);

                // 12. Check for database constraints before saving
                try
                {
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Successfully enrolled student {student.StudentId} in course {courseId}");
                    return Ok(new { message = "Successfully enrolled." });
                }
                catch (DbUpdateException dbEx)
                {
                    _logger.LogError($"Database error during enrollment: {dbEx.Message}");
                    // Get inner exception details
                    var innerMsg = dbEx.InnerException?.Message ?? "No inner exception";
                    return StatusCode(500, new { message = "Database error during enrollment", detail = innerMsg });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in enrollment: {ex.Message}");
                return StatusCode(500, new { message = "An unexpected error occurred", detail = ex.Message });
            }
        }

        // Add a debug endpoint to test request binding
        [HttpPost("debug")]
        public IActionResult DebugRequest([FromBody] object rawRequest)
        {
            try
            {
                var requestJson = System.Text.Json.JsonSerializer.Serialize(rawRequest);

                // Try to deserialize to expected type
                CourseEnrollRequest parsedRequest = null;
                try
                {
                    parsedRequest = System.Text.Json.JsonSerializer.Deserialize<CourseEnrollRequest>(requestJson);
                }
                catch (Exception ex)
                {
                    return BadRequest(new
                    {
                        message = "Failed to parse as CourseEnrollRequest",
                        error = ex.Message
                    });
                }

                return Ok(new
                {
                    receivedRawData = rawRequest,
                    rawJson = requestJson,
                    parsedRequest = parsedRequest,
                    propertyNames = rawRequest.GetType().GetProperties().Select(p => p.Name)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error processing debug request", error = ex.Message });
            }
        }

        public class CourseEnrollRequest
        {
            public Guid CourseId { get; set; }
        }
    }
    }
