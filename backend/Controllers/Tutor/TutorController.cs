using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Tutor;
using backend.Interfaces.Person;
using backend.Repositories.Person;
using EduConnect.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using EduConnect.Entities.Person;
using System.Text;
using System.Security.Cryptography;
using backend.Utilities;
using EduConnect.Entities;
using EduConnect.Entities.Tutor;
using backend.DTOs.Person;
using backend.Interfaces.Tutor;
namespace backend.Controllers.Tutor
{
    [ApiController]
    [Route("/tutor")]
    public class TutorController(DataContext _databaseContext, IPersonRepository _personRepository, ITutorRepository _tutorRepository) : ControllerBase
    {

        [HttpPost("signup")]
        public async Task<IActionResult> RegisterTutor(TutorSignupRequestDTO tutorSingupRequest, DataContext databaseContext)
        {
            Console.WriteLine("Register new tutor with following details: " + tutorSingupRequest.ToString());
            //Check is email taken

            var existingEmail = await _personRepository.GetPersonIdByEmail(tutorSingupRequest.Email);

            //Check is email taken
            if (existingEmail != null)
            {
                return BadRequest("Email is taken");
            }

            //Create new Person
            var Person = new Person
            {
                PersonId = Guid.NewGuid(),
                IsActive = false,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null
            };

            //Create new PersonEmail
            var PersonEmail = new PersonEmail
            {
                PersonId = Person.PersonId,
                PersonEmailId = Guid.NewGuid(),
                Email = tutorSingupRequest.Email,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null

            };



            //Initialize HMACSHA512 hashing algorithm for password hashing
            using var hmac = new HMACSHA512();
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(tutorSingupRequest.Password));

            //Create new PersonPassword 
            var PersonPassword = new PersonPassword
            {
                PersonId = Person.PersonId,
                PersonPasswordId = Guid.NewGuid(),
                Hash = passwordHash,
                Salt = hmac.Key,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null
            };

            //Create new PersonSalt 
            var PersonSalt = new PersonSalt
            {
                PersonSaltId = Guid.NewGuid(),
                PersonId = Person.PersonId,
                NumberOfRounds = 12,
                Salt = EncryptionUtilities.GenerateSalt(),
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null
            };

            //Create new Tutor 
            var Tutor = new EduConnect.Entities.Tutor.Tutor
            {
                PersonId = Person.PersonId,
                Person = Person,
                TutorId = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null
            };


            var savedPersonDataDTO = new PersonEmailPasswordSaltDTOGroup
            {

            };
            var savedTutorDTO = new TutorDTO { };
            //Add Person, PersonEmail, PersonPassword, PersonSalt, Tutor to database
            try
            {
                savedPersonDataDTO = await _personRepository.CreateNewPersonWithHelperTables(
                    Person,
                    PersonEmail,
                    PersonPassword,
                    PersonSalt
                );

                savedTutorDTO = await _tutorRepository.CreateTutor(Tutor);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "We are not able to register you at the moment. Please try again later.");
            }
            //Return created Tutor
            var tutorSignupResponseDTO = new TutorSignupResponseDTO
            {
                TutorId = savedTutorDTO.TutorId,
                Email = savedPersonDataDTO.PersonEmailDTO.Email
            };


            return Ok(new
            {
                message = "Tutor created successfully",
                data = tutorSignupResponseDTO
            });

        }
    }
}