using backend.Interfaces.Person;
using backend.Interfaces.Reference;
using backend.Entities.Person;
using backend.Middleware;
using EduConnect.Data;
using EduConnect.DTOs;
using EduConnect.Entities;
using EduConnect.Entities.Person;
using EduConnect.Entities.Student;
using EduConnect.Extensions;
using EduConnect.Helpers;
using EduConnect.Interfaces;
using EduConnect.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EduConnect.Controllers
{
    public class StudentController(DataContext db, ITokenService _tokenService, IStudentRepository _studentRepo, PersonManager personManager, IPersonRepository _personRepository, IReferenceRepository _referenceRepository) : MainController
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
        public async Task<ActionResult<UserDTO>> RegisterStudent(RegisterStudentDTO request)
        {
            //Check if the email is taken
            var emailExists = await _personRepository.EmailExists(request.Email);

            if (emailExists)
            {
                return Conflict(
                    ApiResponse<object>.GetApiResponse(
                        "Email address is already taken",
                        new { }
                    )
                );
            }



            var countryOfOrigin = await _referenceRepository.GetCountryByName(request.CountryOfOrigin);

            if (countryOfOrigin == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Country of origin not found",
                        new { }
                    )
                );
            }

            var countryOfPhoneNumber = await _referenceRepository.GetCountryByNationalCallingCode(request.PhoneNumberCountryCode);

            if (countryOfPhoneNumber == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "National calling code does not match any country",
                        new { }
                    )
                );
            }



            var hashedPassword = EncryptionUtilities.HashPassword(request.Password);

            var Person = new EduConnect.Entities.Person.Person
            {
                PersonId = Guid.NewGuid(),
                PersonPublicId = Guid.NewGuid(),
                IsActive = false,
                UserName = request.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null

            };

            var PersonEmail = new EduConnect.Entities.Person.PersonEmail
            {
                PersonEmailId = Guid.NewGuid(),
                Email = request.Email,
                PersonId = Person.PersonId,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null
            };

            var PersonPassword = new EduConnect.Entities.Person.PersonPassword
            {
                PersonPasswordId = Guid.NewGuid(),
                PersonId = Person.PersonId,
                PasswordHash = hashedPassword,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null
            };

            var personDetails = new EduConnect.Entities.Person.PersonDetails
            {
                PersonDetailsId = Guid.NewGuid(),
                PersonId = Person.PersonId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                CountryOfOriginCountryId = countryOfOrigin.CountryId,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null
            };

            var personPhoneNumber = new backend.Entities.Person.PersonPhoneNumber
            {
                PersonPhoneNumberId = Guid.NewGuid(),
                PersonId = Person.PersonId,
                NationalCallingCodeCountryId = countryOfPhoneNumber.CountryId,
                PhoneNumber = request.PhoneNumber,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                UpdatedAt = null

            };

            var createPersonResult = await _personRepository.CreatePerson(Person);

            if (!createPersonResult)
            {
                return StatusCode(500, ApiResponse<object>.GetApiResponse("Failed to register you, please try again later", new { }));
            }

            var createPersonEmailResult = await _personRepository.CreatePersonEmail(PersonEmail);
            var createPersonPasswordResult = await _personRepository.CreatePersonPassword(PersonPassword);

            var createPersonDetailsResult = await _personRepository.CreatePersonDetails(personDetails);
            var createPersonPhoneNumberResult = await _personRepository.CreatePersonPhoneNumber(personPhoneNumber);


            if (!createPersonEmailResult || !createPersonPasswordResult || !createPersonDetailsResult || !createPersonPhoneNumberResult)
            {
                return StatusCode(500, ApiResponse<object>.GetApiResponse("Failed to register you, please try again later", new { }));
            }

            var roleAddResult = await personManager.AddToRoleAsync(Person, "Student");

            Console.WriteLine("Was user added to the Student role: " + roleAddResult.Succeeded);

            foreach (var role in roleAddResult.Errors)
            {
                Console.WriteLine(role.Code);
                Console.WriteLine(role.Description);
            }

            //Add to student table
            var addToStudentTable = await db.Student.AddAsync(
                new Entities.Student.Student
                {
                    StudentId = Guid.NewGuid(),
                    PersonId = Person.PersonId,
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    ModifiedAt = null
                }
            );

            Console.WriteLine("Person added to the Student table: " + addToStudentTable.IsKeySet);
            PrintObjectUtility.PrintObjectProperties(addToStudentTable);

            await db.SaveChangesAsync();

            var token = await _tokenService.CreateTokenAsync(Person);


            Response.Headers.Append("Authorization", "Bearer " + token.Token);
            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "You have registered successfully as a student",
                    new { }
                )
            );
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
