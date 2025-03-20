using backend.Middleware;
using EduConnect.Data;
using EduConnect.Entities.Person;
using EduConnect.Entities.Student;
using EduConnect.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EduConnect.Controllers.Student
{
    [ApiController]
    [Route("api/[controller]/Student/")]
    public class SettingsController : ControllerBase
    {
        private readonly DataContext _db;

        public SettingsController(DataContext db)
        {
            _db = db;
        }

        [HttpPost("ChangePassword")]
        [CheckPersonLoginSignup]
        public async Task<ActionResult> ChangePassword([FromQuery] string input)
        {
            var caller = new Caller(this.HttpContext);
            var person = await _db.PersonEmail.FirstOrDefaultAsync(x => x.Email == caller.Email);
            if (person == null)
                return BadRequest(new { message = "Person not found", timestamp = DateTime.UtcNow });

            var currentPasswordRecord = await _db.PersonPassword.FirstOrDefaultAsync(p => p.PersonId == person.PersonId);
            if (currentPasswordRecord == null)
                return BadRequest(new { message = "Password record not found", timestamp = DateTime.UtcNow });

            using (var hmac = new HMACSHA512())
            {
                currentPasswordRecord.Hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                currentPasswordRecord.Salt = hmac.Key;
                currentPasswordRecord.ModifiedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                _db.PersonPassword.Update(currentPasswordRecord);
                await _db.SaveChangesAsync();
            }

            return Ok(new { message = "Password successfully changed", timestamp = DateTime.UtcNow });
        }

       

     
        [HttpPost("ChangeFirstName")]
        [CheckPersonLoginSignup]
        public async Task<ActionResult> ChangeFirstName([FromQuery] string input)
        {
            var caller = new Caller(this.HttpContext);
            var person = await _db.PersonEmail.FirstOrDefaultAsync(x => x.Email == caller.Email);
            if (person == null)
                return BadRequest(new { message = "Person not found", timestamp = DateTime.UtcNow });

            var personDetails = await _db.PersonDetails.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);
            if (personDetails == null)
                return BadRequest(new { message = "Person details not found", timestamp = DateTime.UtcNow });

            personDetails.FirstName = input;
            _db.PersonDetails.Update(personDetails);
            await _db.SaveChangesAsync();

            return Ok(new { message = "First name successfully changed", timestamp = DateTime.UtcNow });
        }

        [HttpPost("ChangeLastName")]
        [CheckPersonLoginSignup]
        public async Task<ActionResult> ChangeLastName([FromQuery] string input)
        {
            var caller = new Caller(this.HttpContext);
            var person = await _db.PersonEmail.FirstOrDefaultAsync(x => x.Email == caller.Email);
            if (person == null)
                return BadRequest(new { message = "Person not found", timestamp = DateTime.UtcNow });

            var personDetails = await _db.PersonDetails.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);
            if (personDetails == null)
                return BadRequest(new { message = "Person details not found", timestamp = DateTime.UtcNow });

            personDetails.LastName = input;
            _db.PersonDetails.Update(personDetails);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Last name successfully changed", timestamp = DateTime.UtcNow });
        }

        [HttpPost("ChangeDescription")]
        [CheckPersonLoginSignup]
        public async Task<ActionResult> ChangeDescription([FromQuery] string input)
        {
            var caller = new Caller(this.HttpContext);
            var person = await _db.PersonEmail.FirstOrDefaultAsync(x => x.Email == caller.Email);
            if (person == null)
                return BadRequest(new { message = "Person not found", timestamp = DateTime.UtcNow });

            var student = await _db.Student.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);
            if (student == null)
                return BadRequest(new { message = "Student not found", timestamp = DateTime.UtcNow });

            var studentDetails = await _db.StudentDetails.FirstOrDefaultAsync(x => x.StudentId == student.StudentId);

            if (studentDetails == null)
            {
                studentDetails = new StudentDetails
                {
                    StudentDetailsId = Guid.NewGuid(),
                    StudentId = student.StudentId,
                    Biography = input,
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    ModifiedAt = null
                };

                await _db.StudentDetails.AddAsync(studentDetails);
            }
            else
            {
                studentDetails.Biography = input;
                studentDetails.ModifiedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                _db.StudentDetails.Update(studentDetails);
            }

            await _db.SaveChangesAsync();

            return Ok(new { message = "Description successfully updated", timestamp = DateTime.UtcNow });
        }

    }
}
