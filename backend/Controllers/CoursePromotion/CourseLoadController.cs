using backend.Middleware;
using EduConnect.Data;
using EduConnect.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Controllers.Promotionz
{
    [ApiController]
    [Route("api/[controller]")]
    [CheckPersonLoginSignup]
    public class CourseLoadController(DataContext _context):ControllerBase
    {

        [HttpGet]
        [Route("/UcitajPromotivniKurs")]
        public async Task<ActionResult> LoadAllCoursesForTutor()
        {
            var caller = new Caller(this.HttpContext);
            var email = caller.Email;

            var person = await _context.PersonEmail
                .FirstOrDefaultAsync(x => x.Email == email);

            if (person == null)
            {
                return NotFound("Person not found.");
            }

            var tutor = await _context.Tutor
                .FirstOrDefaultAsync(x => x.PersonId == person.PersonId);

            if (tutor == null)
            {
                return NotFound("Tutor not found.");
            }

            var courses = await _context.Course
                .Include(x=> x.CourseThumbnail)
                .Where(x => x.TutorId == tutor.TutorId)
                .Select(x => new
                {
                    courseId = x.CourseId,
                    title = x.Title,
                    slika = x.CourseThumbnail.ThumbnailUrl
                })
                .ToListAsync();

            return Ok(courses);
        }

    }
}
