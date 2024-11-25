
using EduConnect.Data;
using EduConnect.DTOs;
using EduConnect.Entities.Person;
using EduConnect.Entities.Student;
using EduConnect.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
        [HttpGet("student/{username}")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentByUsername(string username)
        {
            var students = await _studentRepo.GetStudentInfoByUsername(username);

            if (students == null)
            {

                return NotFound("No students found.");
            }


            return Ok(students);
        }
        [HttpPost("student-login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO login)
        {
            // Retrieve the person's email
            var personDetails = await db.PersonDetails
                .FirstOrDefaultAsync(x => x.Username == login.Username);

            if (personDetails == null)
            {
                return BadRequest("Invalid username.");
            }

            // Retrieve the corresponding person and password details
            var person = await db.Person
                .FirstOrDefaultAsync(x => x.PersonId == personDetails.PersonId);

            var personPassword = await db.PersonPassword
                .FirstOrDefaultAsync(x => x.PersonId == person.PersonId);

            if (personPassword == null)
            {
                return BadRequest("Invalid email or password.");
            }

            // Hash the provided password using the same salt as the stored hash
            using var hmac = new HMACSHA512(personPassword.Salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

            // Compare the computed hash with the stored hash
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != personPassword.Hash[i])
                {
                    return BadRequest("Invalid password.");
                }
            }
            string role = "";
            // Check if the user is a Student or Tutor by looking up the appropriate tables
            var student = await db.Student.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);
            var tutor = await db.Tutor.FirstOrDefaultAsync(x => x.PersonId == person.PersonId);

            if (student != null)
            {
                role = "student";  // User is a student
            }
            else if (tutor != null)
            {
                role = "tutor";  // User is a tutor
            }
            else
            {
                return BadRequest("User is neither a student nor a tutor.");
            }


            var token = _tokenService.CreateTokenAsync(personDetails);

            return new UserDTO
            {
                Username = personDetails.Username,
                Token = token,
                Role=role
            };
        }

        [HttpPost("student-register")]
        public async Task<ActionResult<UserDTO>> RegisterTutor(RegisterStudentDTO student)
        {
            if (await isRegistered(student))
            {
                return BadRequest("That username was already taken");
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
                PhoneNumberCountryCode = student.PhoneNumberCountryCode,
                PhoneNumber = student.PhoneNumber,
                CountryOfOrigin = student.CountryOfOrigin,
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

            var studentt = new Student
            {
                PersonId = Person.PersonId,
                StudentId = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null
            };
            await Task.Run(async () =>
            {
                db.Person.Add(Person);
                db.PersonEmail.Add(PersonEmail);
                db.PersonDetails.Add(PersonDetails);
                db.PersonPassword.Add(PersonPassword);
                db.PersonSalt.Add(PersonSalt);
                db.Student.Add(studentt);
             

                await db.SaveChangesAsync();
            });
            return new UserDTO
            {
                Username = PersonDetails.Username,
                Token = _tokenService.CreateTokenAsync(PersonDetails),
                Role = "student"
            };
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
            return await db.PersonDetails.AnyAsync(x => x.Username == tutor.Username);
        }
    }

}
