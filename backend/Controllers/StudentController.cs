
using backend.Entities.Person;
using backend.Middleware;
using EduConnect.Data;
using EduConnect.DTOs;

using EduConnect.Entities.Person;
using EduConnect.Entities.Student;
using EduConnect.Extensions;
using EduConnect.Helpers;
using EduConnect.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EduConnect.Controllers
{
    public class StudentController(DataContext db, ITokenService _tokenService, IStudentRepository _studentRepo) : MainController
    {
        [HttpGet("all-students")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudents()
        {
            var students = await _studentRepo.GetAllStudents();

            if (students == null || !students.Any())
            {

                return NotFound("No students found.");
            }


            return Ok(students);
        }
        [HttpGet("getCurrentStudentForProfile")]
        [CheckPersonLoginSignup]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentByEmail()
        {
            var caller = new Caller(this.HttpContext);
            var students = await _studentRepo.GetStudentInfoByEmail(caller.Email);

            if (students == null)
            {

                return NotFound("No students found.");
            }


            return Ok(students);
        }
 

        [HttpGet("get-all-emails")]
        public async Task<ActionResult> GetAllEmails()
        {
            try
            {

                var emails = await db.PersonEmail
                                     .Select(x => x.Email)
                                     .ToListAsync();

                if (emails == null || !emails.Any())
                {

                    return NotFound(new
                    {
                        message = "No emails found.",
                        data = new List<string>(),
                        timestamp = DateTime.Now
                    });
                }


                return Ok(new
                {
                    message = "Emails retrieved successfully.",
                    data = emails,
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    message = "An error occurred while fetching emails.",
                    error = ex.Message,
                    timestamp = DateTime.Now
                });
            }
        }

        [HttpPost("student-register")]
        public async Task<IActionResult> RegisterTutor(RegisterStudentDTO student)
        {
            if (await isRegistered(student))
            {
                return BadRequest("That email was already taken");
            }
            var Person = new Person
            {
                PersonId = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null
            };

            var PersonDetails = new PersonDetails
            {
                PersonDetailsId = Guid.NewGuid(),
                PersonId = Person.PersonId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Username = student.Username,
                CountryOfOriginCountryId = student.CountryOfOriginCountryId,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null



            };
            var PersonEmail = new PersonEmail
            {
                PersonEmailId = Guid.NewGuid(),
                PersonId = Person.PersonId,
                Email = student.Email,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null


            };
            using var hmac = new HMACSHA512();
            var vhashi = hmac.ComputeHash(Encoding.UTF8.GetBytes(student.Password));
            var PersonPassword = new PersonPassword
            {
                PersonPasswordId = Guid.NewGuid(),
                PersonId = Person.PersonId,
                Hash = vhashi,
                Salt = hmac.Key,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null
            };

            var NumberofR = 10000;
            var PersonSalt = new PersonSalt
            {
                PersonSaltId = Guid.NewGuid(),
                PersonId = Person.PersonId,
                NumberOfRounds = NumberofR,
                Salt = GenerateSalt(),
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null
            };
            var PhoneNumber = new PersonPhoneNumber
            {
                PersonId = Person.PersonId,
                NationalCallingCodeCountryId = student.PhoneNumberCountryCodeCountryId,
                PhoneNumber = student.PhoneNumber,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                UpdatedAt = null


            };

            var studentt = new Entities.Student.Student
            {
                PersonId = Person.PersonId,
                StudentId = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null
            };

            //Check does PhoneNumberCountryCodeCountryId exist
            var countryByCallingCode = await db.Country.Where(x => x.CountryId == student.PhoneNumberCountryCodeCountryId).FirstOrDefaultAsync();
            if (countryByCallingCode == null) {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "National calling code does not exist",
                        data = new { },
                        timestamp = DateTime.Now,
                    });
            }

            //Check does countryOfOrigin exist
            var countryByCountryOfOriginCountryId = await db.Country.Where(
                x => x.CountryId == student.CountryOfOriginCountryId
            )
            .FirstOrDefaultAsync();

            if (countryByCountryOfOriginCountryId == null) { 
                    return NotFound(
                    new
                    {
                        success = "false",
                        message = "Country does not exist",
                        data = new { },
                        timestamp = DateTime.Now,
                    });
            }
            await Task.Run(async () =>
            {
                db.Person.Add(Person);
                db.PersonEmail.Add(PersonEmail);
                db.PersonDetails.Add(PersonDetails);
                db.PersonPassword.Add(PersonPassword);
                db.PersonSalt.Add(PersonSalt);
                db.PersonPhoneNumber.Add(PhoneNumber);
                db.Student.Add(studentt);
                


                await db.SaveChangesAsync();
            });
            Response.Headers["Authorization"] = await _tokenService.CreateTokenAsync(PersonEmail);
            return Ok(new {
            message = "User was sucessfully created",
               data = new UserDTO
               {
                   Email = PersonEmail.Email,
                   Token = await _tokenService.CreateTokenAsync(PersonEmail),
                   Role = "student"
              
               },
            });
        }
        private string GenerateSalt()
        {
            var saltBytes = new byte[16];


            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }
        private async Task<bool> isRegistered(RegisterStudentDTO tutor)
        {
            return await db.PersonEmail.AnyAsync(x => x.Email == tutor.Email);
        }
    }

}
