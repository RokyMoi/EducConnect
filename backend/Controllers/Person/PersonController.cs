using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using backend.DTOs.Person;
using backend.DTOs.Person.PersonAvailability;
using backend.DTOs.Person.PersonDetails;
using backend.DTOs.Tutor;
using backend.Interfaces.Person;
using backend.Interfaces.Tutor;
using backend.Middleware;
using backend.Repositories.Person;
using EduConnect.Data;
using EduConnect.DTOs;
using EduConnect.Entities.Person;
using EduConnect.Entities.Student;
using EduConnect.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers.Person
{
    [ApiController]
    [Route("person")]
    public class PersonController(DataContext db, ITokenService _tokenService, IStudentRepository _studentRepo, IPersonRepository _personRepository, ITutorRepository _tutorRepository, IPersonAvailabilityRepository _personAvailability, IPersonCareerInformationRepository _personCareerInformationRepository, IPersonEducationInformationRepository _personEducationInformationRepository) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {


            // Retrieve the person's email
            var personEmail = await db.PersonEmail.Include(x => x.Person).Where(x => x.Email == login.Email)
                .FirstOrDefaultAsync();

            if (personEmail == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "User not found",
                    data = new { },
                    timestamp = DateTime.Now,

                });
            }



            // Retrieve the corresponding person and password details
            var personPassword = await db.PersonPassword
    .FirstOrDefaultAsync(x => x.PersonId == personEmail.PersonId);

            if (personPassword == null)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Username or password invalid",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }

            // Hash the provided password using the same salt as the stored hash
            using var hmac = new HMACSHA512(personPassword.Salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

            // Compare the computed hash with the stored hash
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != personPassword.Hash[i])
                {
                    return BadRequest(new
                    {
                        success = "false",
                        message = "Username or password invalid",
                        data = new { },
                        timestamp = DateTime.Now,
                    });
                }
            }
            string role = "";
            // Check if the user is a Student or Tutor by looking up the appropriate tables
            var student = await db.Student.FirstOrDefaultAsync(x => x.PersonId == personEmail.PersonId);
            var tutor = await db.Tutor.FirstOrDefaultAsync(x => x.PersonId == personEmail.PersonId);

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
                return Unauthorized(new
                {
                    success = "error",
                    message = "Role undefined",
                    data = new { },
                    timestamp = DateTime.Now,
                });
            }


            var token = await _tokenService.CreateTokenAsync(personEmail);


            //Add the token to the response authorization header
            HttpContext.Response.Headers.Authorization = $"Bearer {token}";
            return
            Ok(
                new
                {
                    success = "true",
                    message = "Login succesfull",
                    data = new

            UserDTO
                    {
                        Email = personEmail.Email,
                        Token = token,
                        Role = role,
                    },
                    timestamp = DateTime.Now,
                });

        }

        [HttpGet("signup/get")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> GetSignupData()
        {
            Console.WriteLine("HttpContext email: " + HttpContext.Items["Email"].ToString());

            //Check if the email in the context dictionary is null
            if (string.IsNullOrEmpty(HttpContext.Items["Email"].ToString()))
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "error",
                        message = "Something went wrong, please try again later.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            string email = HttpContext.Items["Email"].ToString();

            Guid personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());
            //Check if the PersonId from dictionary is null and if it is, call to the database to get the PersonId
            if (string.IsNullOrEmpty(HttpContext.Items["PersonId"].ToString()))
            {
                var personObjectEmail = await _personRepository.GetPersonEmailByEmail(email);
                personId = personObjectEmail.PersonId;
            }

            if (personId == Guid.Empty)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "error",
                        message = "Something went wrong, please try again later.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }
            //Check if the PersonId is Tutor and if it is, check the TutorRegistrationStatus

            var tutor = await _tutorRepository.GetTutorRegistrationStatusByPersonId(personId);
            Console.WriteLine("Is person a tutor: " + tutor != null);
            //If tutor is not null, check the TutorRegistrationStatus is below 6 (Email Verification, status before)
            if (tutor != null && tutor.TutorRegistrationStatusId < 7)
            {
                return UnprocessableEntity(
                    new
                    {

                        success = "false",
                        message = "It looks like you haven't completed your tutor registration yet. Please complete it to continue.",
                        data = new
                        {
                            CurrentTutorRegistrationStatus = new
                            {
                                TutorRegistrationStatusId = tutor.TutorRegistrationStatusId,

                            }
                        },
                        timestamp = DateTime.Now

                    }
                );
            }
            //If tutor is null, check if the person is a student
            Guid studentId = Guid.Empty;
            if (tutor == null)
            {
                var studentDTO = await _studentRepo.GetStudentByPersonId(personId);
                if (studentDTO == null)
                {
                    return StatusCode(
                        500,
                        new
                        {
                            success = "error",
                            message = "Something went wrong, please try again later.",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }
                studentId = studentDTO.StudentId;
                personId = studentDTO.PersonId;
            }

            //Get data for the person from the database

            //Get data for PersonAvailibility
            var personAvailabilityList = await _personAvailability.GetAllPersonAvailabilityByPersonId(personId);
            var personAvailabilityDTOList = new List<PersonAvailabilitySaveResponseDTO>();
            foreach (var personAvailability in personAvailabilityList)
            {
                personAvailabilityDTOList.Add(new PersonAvailabilitySaveResponseDTO
                {
                    PersonAvailabilityId = personAvailability.PersonAvailabilityId,
                    DayOfWeek = personAvailability.DayOfWeek,
                    StartTime = personAvailability.StartTime,
                    EndTime = personAvailability.EndTime,
                });
            }



            //Get data for PersonCareerInformation
            var personCareerInformationList = await _personCareerInformationRepository.GetAllPersonCareerInformationByPersonId(personId);
            var personCareerInformationDTOList = new List<PersonCareerInformationSaveResponseDTO>();
            foreach (var personCareerInformation in personCareerInformationList)
            {
                personCareerInformationDTOList.Add(new PersonCareerInformationSaveResponseDTO
                {
                    PersonCareerInformationId = personCareerInformation.PersonCareerInformationId,
                    CompanyName = personCareerInformation.CompanyName,
                    CompanyWebsite = personCareerInformation.CompanyWebsite,
                    JobTitle = personCareerInformation.JobTitle,
                    Position = personCareerInformation.Position,
                    CityOfEmployment = personCareerInformation.CityOfEmployment,
                    CountryOfEmployment = personCareerInformation.CountryOfEmployment,
                    StartDate = personCareerInformation.StartDate,
                    EndDate = personCareerInformation.EndDate,
                    JobDescription = personCareerInformation.JobDescription,
                    Responsibilities = personCareerInformation.Responsibilities,
                    Achievements = personCareerInformation.Achievements,
                    IndustryClassificationId = personCareerInformation.IndustryClassificationId,
                    Industry = personCareerInformation.Industry,
                    Sector = personCareerInformation.Sector,
                    SkillsUsed = personCareerInformation.SkillsUsed,
                    WorkTypeId = personCareerInformation.WorkTypeId,
                    AdditionalInformation = personCareerInformation.AdditionalInformation,



                });
            }
            //Get data for PersonDetails
            var personDetails = await _personRepository.GetPersonDetailsByPersonId(personId);

            var personDetailsDTO = new PersonDetailsSaveResponseDTO
            {
                PersonDetailsId = personDetails.PersonDetailsId,
                FirstName = personDetails.FirstName,
                LastName = personDetails.LastName,
                Username = personDetails.Username,

            };

            //Get data for PersonEducationInformation
            var personEducationInformation = await _personEducationInformationRepository.GetAllPersonEducationInformationByPersonId(personId);
            var personEducationInformationDTOList = new List<PersonEducationInformationResponseDTO>();
            foreach (var personEducationInformationItem in personEducationInformation)
            {
                personEducationInformationDTOList.Add(
                    new PersonEducationInformationResponseDTO
                    {
                        PersonEducationInformationId = personEducationInformationItem.PersonEducationInformationId,
                        InstitutionName = personEducationInformationItem.InstitutionName,
                        InstitutionOfficialWebsite = personEducationInformationItem.InstitutionOfficialWebsite,
                        InstitutionAddress = personEducationInformationItem.InstitutionAddress,
                        EducationLevel = personEducationInformationItem.EducationLevel,
                        FieldOfStudy = personEducationInformationItem.FieldOfStudy,
                        MinorFieldOfStudy = personEducationInformationItem.MinorFieldOfStudy,
                        StartDate = personEducationInformationItem.StartDate,
                        EndDate = personEducationInformationItem.EndDate,
                        IsCompleted = personEducationInformationItem.IsCompleted,
                        FinalGrade = personEducationInformationItem.FinalGrade,
                        Description = personEducationInformationItem.Description
                    }
                );
            }

            //Get data for PersonEmail
            var personEmail = await _personRepository.GetPersonEmailByEmail(email);
            var personEmailDTO = new PersonEmailResponseDTO
            {
                PersonEmailId = personEmail.PersonEmailId,
                Email = personEmail.Email,
            };

            TutorTeachingInformationWithIncludedObjectsDTO? tutorTeachingInformation = null;
            //If the person is a tutor, get the data specific to the Tutor role
            if (tutor != null)
            {
                //Get data for Tutor
                tutorTeachingInformation = await _tutorRepository.GetTutorTeachingInformationWithIncludedObjectsByTutorId(tutor.TutorId);

            }

            return Ok(
                new
                {
                    success = "true",
                    message = "Data for this account retrieved successfully",
                    data = new
                    {
                        Person = new
                        {
                            PersonDetails = personDetailsDTO,
                            PersonCareerInformationList = personCareerInformationDTOList,
                            PersonEducationInformation = personEducationInformationDTOList,
                            PersonEmail = personEmailDTO,
                            PersonAvailability = personAvailabilityDTOList,
                        },
                        Tutor = tutorTeachingInformation,


                    }

                }
            );
        }

        [HttpPut("signup/complete")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> CompleteSignUp([FromQuery] bool completeSignup)
        {


            //Check if the email in the context dictionary is null
            if (string.IsNullOrEmpty(HttpContext.Items["Email"].ToString()))
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "error",
                        message = "Something went wrong, please try again later.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            string email = HttpContext.Items["Email"].ToString();

            Guid personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());
            //Check if the PersonId from dictionary is null and if it is, call to the database to get the PersonId
            if (string.IsNullOrEmpty(HttpContext.Items["PersonId"].ToString()))
            {
                var personObjectEmail = await _personRepository.GetPersonEmailByEmail(email);
                personId = personObjectEmail.PersonId;
            }

            if (personId == Guid.Empty)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "error",
                        message = "Something went wrong, please try again later.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }
            //Check if the PersonId is Tutor and if it is, check the TutorRegistrationStatus

            var tutor = await _tutorRepository.GetTutorRegistrationStatusByPersonId(personId);
            Console.WriteLine("Is person a tutor: " + tutor != null);
            //If tutor is not null, check the TutorRegistrationStatus is below 6 (Email Verification, status before)
            if (tutor != null && tutor.TutorRegistrationStatusId < 8)
            {
                return UnprocessableEntity(
                    new
                    {

                        success = "false",
                        message = "It looks like you haven't completed your tutor registration yet. Please complete it to continue.",
                        data = new
                        {
                            CurrentTutorRegistrationStatus = new
                            {
                                TutorRegistrationStatusId = tutor.TutorRegistrationStatusId,

                            }
                        },
                        timestamp = DateTime.Now

                    }
                );
            }
            //If tutor is null, check if the person is a student
            Guid studentId = Guid.Empty;
            if (tutor == null)
            {
                var studentDTO = await _studentRepo.GetStudentByPersonId(personId);
                if (studentDTO == null)
                {
                    return StatusCode(
                        500,
                        new
                        {
                            success = "error",
                            message = "Something went wrong, please try again later.",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }
                studentId = studentDTO.StudentId;
                personId = studentDTO.PersonId;
            }

            if (!completeSignup)
            {
                return Ok(
                    new
                    {
                        success = "true",
                        message = "Signup complete request canceled",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //If person is a tutor, update the TutorRegistrationStatus to 9 (Registration Completed)

            if (tutor != null)
            {
                var updateResult = await _tutorRepository.UpdateTutorRegistrationStatus(personId, 9);
                if (updateResult == null)
                {
                    return StatusCode(
                        500,
                        new
                        {
                            success = "error",
                            message = "Something went wrong, please try again later.",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }
            }

            return Ok(
                new
                {
                    success = "true",
                    message = "You have successfully completed your registration process. Welcome onboard.",
                    data = new
                    {
                    },
                    timestamp = DateTime.Now
                }
            );


        }


    }
}