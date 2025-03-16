using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
using EduConnect.Entities;
using EduConnect.Entities.Person;
using EduConnect.Entities.Student;
using EduConnect.Interfaces;
using EduConnect.Middleware;
using EduConnect.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers.Person
{
    [ApiController]
    [Route("person")]
    public class PersonController(DataContext db, ITokenService _tokenService, IStudentRepository _studentRepo, IPersonRepository _personRepository, ITutorRepository _tutorRepository, IPersonAvailabilityRepository _personAvailability, IPersonCareerInformationRepository _personCareerInformationRepository, IPersonEducationInformationRepository _personEducationInformationRepository, UserManager<EduConnect.Entities.Person.Person> _userManager, IHttpContextAccessor _httpContextAccessor) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest login)
        {

            Console.WriteLine("Login user with username or email: " + login.UsernameOrEmail);
            Console.WriteLine("Login user with password: " + login.Password);

            var person = await _personRepository.GetPersonByEmailOrUsername(login.UsernameOrEmail);


            if (person == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "User not found",
                        new { }
                    )
                );
            }

            var personPassword = await _personRepository.GetPersonPasswordByPersonId(person.PersonId);

            if (personPassword == null)
            {
                return StatusCode(500,
                ApiResponse<object>.GetApiResponse(
                        "We could not log you in, please try again later",
                        new { }
                ));
            }
            var passwordComparisonResult = EncryptionUtilities.VerifyHashedPassword(personPassword.PasswordHash, login.Password);

            Console.WriteLine("Password comparison result: " + passwordComparisonResult);

            if (!passwordComparisonResult)
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse(
                        "Password is incorrect",
                        new
                        {
                            personPassword.PasswordHash
                        }
                    )
                );

            }

            var roles = await _personRepository.GetRolesByPersonId(person.PersonId);

            Console.WriteLine("Roles for person: " + login.UsernameOrEmail + " - " + roles == null);
            foreach (var role in roles)
            {
                Console.WriteLine("Role for person: " + login.UsernameOrEmail + " - Role name: " + role.Name + " - Role id: " + role.Id);
            }
            if (await _tokenService.CheckIfExistsByPersonId(person.PersonId))
            {
                await _tokenService.RevokeTokenByPersonId(person.PersonId);

            }

            var token = await _tokenService.CreateTokenAsync(person);

            Response.Headers.Append("Authorization", "Bearer " + token.Token);


            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "You have logged in successfully",
                    new
                    {
                        role = roles.FirstOrDefault().Name,
                    }
                )
            );



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

        [HttpGet("authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return BadRequest("Missing HttpContext");
            }

            var token = httpContext.Request.Headers.Authorization.ToString().Replace("Bearer", "").Trim();

            Console.WriteLine("Request token: ");
            Console.WriteLine(token);

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Missing token");
            }

            var validationResult = await _tokenService.ValidateToken(token);

            if (!validationResult)
            {
                return Unauthorized("Invalid token");
            }

            var userPublicIdFromClaims = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(
                            x => x.Type == ClaimTypes.NameIdentifier
                        )?.Value;
            var publicUserId = Guid.Parse(userPublicIdFromClaims);
            var user = await _personRepository.GetPersonByPublicPersonId(publicUserId);

            var roles = await _personRepository.GetRolesByPersonId(user.PersonId);

            var roleNames = roles.Select(role => role.Name).ToList();


            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "User is already logged in",
                    new
                    {
                        role = roleNames.FirstOrDefault().ToLower(),
                    }
                )
            );
        }
        [HttpPost("role")]
        [AuthenticationGuard(isTutor: true, isAdmin: true, isStudent: true)]
        public async Task<IActionResult> CheckUserRole([FromBody] CheckUserRoleRequest request)
        {
            if (string.IsNullOrEmpty(request.RequiredRole.Trim()))
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse(
                        "Role must be provided",
                        null
                    )
                );
            }

            var userPublicIdFromClaims = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(
                            x => x.Type == ClaimTypes.NameIdentifier
                        )?.Value;

            Console.WriteLine($"User public Id: {userPublicIdFromClaims}");

            Guid userPublicId = Guid.Empty;
            if (string.IsNullOrEmpty(userPublicIdFromClaims) || !Guid.TryParse(userPublicIdFromClaims, out userPublicId))
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse(
                        "Missing user public id",
                        new { })
                );
            }

            Console.WriteLine($"Parse User Public Id: {userPublicId}");
            var publicUserId = userPublicId;
            var user = await _personRepository.GetPersonByPublicPersonId(userPublicId);

            if (user == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "User not found",
                        new { })
                );

            }

            var roles = await _personRepository.GetRolesByPersonId(user.PersonId);

            if (!roles.Any(x => x.NormalizedName.Equals(request.RequiredRole, StringComparison.CurrentCultureIgnoreCase)))
            {
                return StatusCode(
                    403,
                    ApiResponse<object>.GetApiResponse(
                        "You do not have the required role",
                        new { })

                );
            }

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "You have the required role",
                    new
                    {

                    })
            );


        }

        [HttpGet("protected/student")]
        [AuthenticationGuard(isTutor: false, isAdmin: false, isStudent: true)]
        public async Task<IActionResult> StudentProtectedRoute()
        {


            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "This is a protected route for students",
                    null
                )
            );

        }

        [HttpGet("protected/tutor")]
        [AuthenticationGuard(isTutor: true, isAdmin: false, isStudent: false)]
        public async Task<IActionResult> TutorProtectedRoute()
        {

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "This is a protected route for tutors",
                    null
                )
            );

        }

        [HttpGet("protected/admin")]
        [AuthenticationGuard(isTutor: false, isAdmin: true, isStudent: false)]
        public async Task<IActionResult> AdminProtectedRoute()
        {

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "This is a protected route for tutors",
                    null
                )
            );

        }

        [HttpDelete("logout")]
        [AuthenticationGuard(isTutor: true, isAdmin: true, isStudent: true)]
        public async Task<IActionResult> Logout()
        {
            var userPublicIdFromClaims = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(
                x => x.Type == ClaimTypes.NameIdentifier
            )?.Value;

            Console.WriteLine($"User public Id: {userPublicIdFromClaims}");

            Guid userPublicId = Guid.Empty;
            if (string.IsNullOrEmpty(userPublicIdFromClaims) || !Guid.TryParse(userPublicIdFromClaims, out userPublicId))
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse(
                        "Missing user public id",
                        new { })
                );
            }

            Console.WriteLine($"Parse User Public Id: {userPublicId}");
            var publicUserId = userPublicId;
            var user = await _personRepository.GetPersonByPublicPersonId(userPublicId);

            if (user == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "User not found",
                        new { })
                );
            }


            var deleteResult = await _tokenService.RevokeTokenByPersonId(user.PersonId);

            if (!deleteResult)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "Failed to log out",
                        new { })
                );
            }

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Logout successful",
                    new { })
            );

        }

        [HttpGet("dashboard/info/user")]
        [AuthenticationGuard(isTutor: true, isAdmin: true, isStudent: true)]
        public async Task<IActionResult> GetDashboardUserInfo()
        {
            var userPublicIdFromClaims = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(
                x => x.Type == ClaimTypes.NameIdentifier
            )?.Value;

            Console.WriteLine($"User public Id: {userPublicIdFromClaims}");

            Guid userPublicId = Guid.Empty;
            if (string.IsNullOrEmpty(userPublicIdFromClaims) || !Guid.TryParse(userPublicIdFromClaims, out userPublicId))
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse(
                        "Missing user public id",
                        new { })
                );
            }

            Console.WriteLine($"Parse User Public Id: {userPublicId}");
            var publicUserId = userPublicId;
            var user = await _personRepository.GetPersonByPublicPersonId(userPublicId);

            if (user == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "User not found",
                        new { })
                );
            }

            var personInfo = await _personRepository.GetDashboardPersonInfo(user.PersonId);

            if (personInfo == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Data for user not found",
                        new { })
                );

            }

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "User info",
                    personInfo
                )
            );






        }



    }
}