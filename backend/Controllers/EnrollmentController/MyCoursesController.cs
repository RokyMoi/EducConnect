using backend.Middleware;
using EduConnect.Data;
using EduConnect.Entities.Course;
using EduConnect.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EduConnect.Controllers.MyCoursesController
{
    [ApiController]
    [Route("Info")]
    [CheckPersonLoginSignup]
    public class MyCoursesController(DataContext _context) : ControllerBase
    {
        [HttpGet("my-courses")]
        public async Task<ActionResult<object>> GetMyCourses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var caller = new Caller(HttpContext);
            var email = caller.Email;
            var personEmail = _context.PersonEmail.FirstOrDefault(x => x.Email == email);

            // Pronađi studenta na osnovu emaila
            var student = await _context.Student.FirstOrDefaultAsync(s => s.PersonId == personEmail.PersonId);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            // Query for the enrollments with pagination
            var query = _context.StudentEnrollment
                .Include(e => e.Course)
                    .ThenInclude(c => c.CourseThumbnail)
                .Where(e => e.StudentId == student.StudentId && e.Status == EnrollmentStatus.Active);

            // Get total count for pagination metadata
            var totalCount = await query.CountAsync();

            // Apply pagination
            var enrollments = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new
                {
                    CourseId = e.CourseId,
                    Title = e.Course.Title,
                    Description = e.Course.Description,
                    ThumbnailUrl = e.Course.CourseThumbnail != null ? e.Course.CourseThumbnail.ThumbnailUrl : null,
                    Category = e.Course.CourseCategory.Name
                })
                .ToListAsync();

            // Return paginated result with metadata
            return Ok(new
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Data = enrollments
            });
        }
    }
}