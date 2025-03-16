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
using EduConnect.Entities;
using EduConnect.Entities.Tutor;
using backend.DTOs.Person;
using backend.Interfaces.Tutor;
using backend.Services;
using backend.Entities.Person;
using EduConnect.DTOs;
using Newtonsoft.Json.Linq;
using EduConnect.Interfaces;
using backend.Interfaces.Reference;
using backend.Entities.Reference.Country;
using backend.Middleware.Tutor;
using backend.Middleware;
using backend.DTOs.Tutor;
using Microsoft.Extensions.ObjectPool;
using Microsoft.AspNetCore.Identity;
using EduConnect.Utilities;

namespace backend.Controllers.Tutor
{
    [ApiController]
    [Route("/tutor")]
    public class TutorController(DataContext _databaseContext, ITokenService _tokenService, IPersonRepository _personRepository, ITutorRepository _tutorRepository, ICountryRepository _countryRepository, IReferenceRepository _referenceRepository, PersonManager personManager) : ControllerBase
    {

        [HttpPost("signup")]
        public async Task<IActionResult> RegisterTutor(RegisterTutorRequest request)
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

            var hashedPassword = EncryptionUtilities.HashPassword(request.Password);

            var Person = new EduConnect.Entities.Person.Person
            {
                PersonId = Guid.NewGuid(),
                PersonPublicId = Guid.NewGuid(),
                IsActive = false,
                UserName = request.Email.Split("@")[0],
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

            var createPersonResult = await _personRepository.CreatePerson(Person);

            if (!createPersonResult)
            {
                return StatusCode(500, ApiResponse<object>.GetApiResponse("Failed to register you, please try again later", new { }));
            }

            var createPersonEmailResult = await _personRepository.CreatePersonEmail(PersonEmail);
            var createPersonPasswordResult = await _personRepository.CreatePersonPassword(PersonPassword);

            if (!createPersonEmailResult || !createPersonPasswordResult)
            {
                return StatusCode(500, ApiResponse<object>.GetApiResponse("Failed to register you, please try again later", new { }));
            }

            var roleAddResult = await personManager.AddToRoleAsync(Person, "Tutor");

            Console.WriteLine("Was user added to the Tutor role: " + roleAddResult.Succeeded);

            foreach (var role in roleAddResult.Errors)
            {
                Console.WriteLine(role.Code);
                Console.WriteLine(role.Description);
            }

            var token = await _tokenService.CreateTokenAsync(Person);


            Response.Headers.Append("Authorization", "Bearer " + token.Token);

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "You have registered successfully as a tutor",
                    new { }
                )
            );
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyTutorEmail(TutorVerifyVerificationCodeRequestDTO verificationCodeRequestDTO, DataContext databaseContext)
        {



            //Check if the email exists
            var existingEmail = await _personRepository.GetPersonEmailByEmail(verificationCodeRequestDTO.Email);

            if (existingEmail == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Email address is not a registered email address",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Check the verification code
            var verificationCodeFromDatabase = await _personRepository.GetPersonVerificationCodeByEmail(verificationCodeRequestDTO.Email);

            if (verificationCodeFromDatabase == null)
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "Given email address is not registered for verification",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            if (verificationCodeFromDatabase.ExpiryDateTime < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Verification code has expired, please request a new verification code",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check is the verification code already verified
            if (verificationCodeFromDatabase.IsVerified)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "This email address has already been verified",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }
            if (verificationCodeFromDatabase.VerificationCode != verificationCodeRequestDTO.VerificationCode)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Verification code is incorrect",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Set person account as verified
            var updatedValue = await _personRepository.VerifyPersonVerificationCode(verificationCodeFromDatabase);

            if (updatedValue == null)
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "Error while updating person verification code",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            Console.WriteLine(verificationCodeFromDatabase.PersonId);
            //Update the Tutor registration status to 2
            var tutor = await _tutorRepository.GetTutorRegistrationStatusByPersonId(verificationCodeFromDatabase.PersonId);
            var updatedTutorRegistrationStatus = await _tutorRepository.UpdateTutorRegistrationStatus(tutor.PersonId, 2);

            return Ok(new
            {
                success = "true",
                message = "Email address has been verified successfully",
                data = new { },
                timestamp = DateTime.Now
            });


        }

        [HttpPost("resend-verification-code")]
        public async Task<IActionResult> ResendVerificationCode(TutorResendVerificationCodeRequestDTO resendVerificationCodeRequestDTO, DataContext databaseContext)
        {

            //Check if the email exists
            var personEmail = await _personRepository.GetPersonEmailWithPersonObjectByEmail(resendVerificationCodeRequestDTO.Email);

            if (personEmail == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Email address not found",
                    data = new { },
                    timestamp = DateTime.Now,
                });
            }

            //Get verification code from the database
            var verificationCodeFromDatabase = await _personRepository.GetPersonVerificationCodeByEmail(resendVerificationCodeRequestDTO.Email);

            //Check is there already a verification code for the given email address
            //If no verification code was found for the given email address, new verification code will be generated and email sent to the user with given email address
            if (verificationCodeFromDatabase == null)
            {
                //Generate verification code
                var newPersonVerificationCode = new PersonVerificationCode
                {
                    PersonVerificationCodeId = Guid.NewGuid(),
                    PersonId = personEmail.PersonId,
                    Person = personEmail.Person,
                    VerificationCode = EncryptionUtilities.GenerateRandomString(),
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    ModifiedAt = null
                };

                var verificationCodeDatabaseResult = await _personRepository.CreateNewPersonVerificationCode(newPersonVerificationCode);

                if (verificationCodeDatabaseResult == null)
                {
                    return StatusCode(500, new
                    {
                        success = "false",
                        message = "Code could not be sent, please try again later",
                        data = new { },
                        timestamp = DateTime.Now
                    });

                }

                var emailSendingResult = await EmailService.SendEmailToAsync(resendVerificationCodeRequestDTO.Email, "EduConnect - Verification Code Resend",

                    "Hello, \n\n"
                    + "A new verification code has been requested to be sent to this email for only purpose of verification of the given email \n\n"
                    + $"Your new verification code is: {verificationCodeDatabaseResult.VerificationCode}\n\n"
                    + $"Notice: This code expires at {DateTimeOffset.FromUnixTimeMilliseconds(verificationCodeDatabaseResult.ExpiryDateTime).ToUniversalTime().ToString("HH:mm:ss dd.MM.yyyy.  'UTC'")}\n\n"
                    + "Please use this code to verify your email for the usage the platform.\n\n"
                    + "If you didn’t sign up for EduConnect, or didn't request verification code resend, please ignore this email.\n\n"
                    + "Thank you for choosing EduConnect as your platform to spread knowledge. Together, we’re shaping the future of education.\n\n"
                    + "Best regards,\n"
                    + "The EduConnect Team\n\n"
                    + "P.S. Need help or have questions? Feel free to reach out to us at support@educonnect.com.\n"
            );
                if (!emailSendingResult)
                {
                    return BadRequest(new
                    {
                        success = "false",
                        message = "Code could not be sent, please try again later",
                        data = new { },
                        timestamp = DateTime.Now
                    });
                }

                return Ok(new
                {
                    success = "true",
                    message = "New verification code has been sent, please check your email inbox",
                    data = new { },
                    timestamp = DateTime.Now
                });


            }

            //If verification code was found, the existing verification code will be deleted and a new one will be generated, and sent to the user

            //Check is the existing verification code already verified
            if (verificationCodeFromDatabase.IsVerified)
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "User with given email address is already verified",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Check is the existing verification code expired
            if (verificationCodeFromDatabase.ExpiryDateTime > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "A valid verification code has already been sent your email address, please check your email inbox/spam",
                    data = new { },
                    timestamp = DateTime.Now
                });

            }


            //Generate verification code
            var PersonVerificationCode = new PersonVerificationCode
            {
                PersonVerificationCodeId = Guid.NewGuid(),
                PersonId = personEmail.PersonId,
                Person = personEmail.Person,
                VerificationCode = EncryptionUtilities.GenerateRandomString(),
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null
            };


            //Delete the existing verification code from the database
            var verificationCodeDeleteResult = await _personRepository.DeletePersonVerificationCodeByPersonId(verificationCodeFromDatabase.PersonId);
            if (!verificationCodeDeleteResult)
            {
                return StatusCode(500, new
                {
                    success = "false",
                    message = "Code could not be sent, please try again later",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Add new verification code to the database
            var addToTheDatabaseResult = await _personRepository.CreateNewPersonVerificationCode(PersonVerificationCode);
            if (addToTheDatabaseResult == null)
            {
                return StatusCode(500, new
                {
                    success = "false",
                    message = "Code could not be sent, please try again later",
                    data = new { },
                    timestamp = DateTime.Now
                });

            }

            //Send email with the new verification code
            var emailResult = await EmailService.SendEmailToAsync(resendVerificationCodeRequestDTO.Email, "EduConnect - Verification Code Resend",

                  "Hello, \n\n"
                  + "A new verification code has been requested to be sent to this email for only purpose of verification of the given email \n\n"
                  + $"Your new verification code is: {addToTheDatabaseResult.VerificationCode}\n\n"
                  + $"Notice: This code expires at {DateTimeOffset.FromUnixTimeMilliseconds(addToTheDatabaseResult.ExpiryDateTime).ToUniversalTime().ToString("HH:mm:ss dd.MM.yyyy.  'UTC'")}\n\n"
                  + "Please use this code to verify your email for the usage the platform.\n\n"
                  + "If you didn’t sign up for EduConnect, or didn't request verification code resend, please ignore this email.\n\n"
                  + "Thank you for choosing EduConnect as your platform to spread knowledge. Together, we’re shaping the future of education.\n\n"
                  + "Best regards,\n"
                  + "The EduConnect Team\n\n"
                  + "P.S. Need help or have questions? Feel free to reach out to us at support@educonnect.com.\n"
          );


            //Check if the email was sent
            if (!emailResult)
            {
                return StatusCode(500, new
                {
                    success = "false",
                    message = "Code could not be sent, please try again later",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            return Ok(new
            {
                success = "true",
                message = "New verification code has been sent, please check your email inbox",
                data = new { },
                timestamp = DateTime.Now
            });

        }

        [HttpGet("signup/status")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> getTutorRegistrationStatus()
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


            //Check if the personId exists in the Tutor table
            var tutor = await _tutorRepository.GetTutorByPersonId(personId);

            if (tutor == null)
            {
                return StatusCode(
                    403,
                    new
                    {
                        success = "false",
                        message = "You are not a tutor",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Get the TutorRegistrationStatus data
            var tutorRegistrationStatus = await _tutorRepository.GetTutorRegistrationStatusByPersonId(personId);

            if (tutorRegistrationStatus == null)
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

            //Convert the TutorRegistrationStatus to TutorRegistrationStatusResponseDTO

            return Ok(
                new
                {
                    success = "true",
                    message = "Tutor registration status retrieved successfully",
                    data = new
                    {
                        tutorRegistrationStatus = new
                       TutorRegistrationStatusResponseDTO
                        {
                            TutorId = tutor.TutorId,
                            TutorRegistrationStatusId = tutorRegistrationStatus.TutorRegistrationStatusId,
                            Name = tutorRegistrationStatus.TutorRegistrationStatusName,
                            Description = tutorRegistrationStatus.TutorRegistrationStatusDescription,
                            IsSkippable = tutorRegistrationStatus.IsSkippable

                        },
                        timestamp = DateTime.Now,
                    }
                }
            );

        }

        // //Method for saving Tutor's personal information using Person.PersonDetail table
        // [HttpPost("/tutor/signup/personal-info/add")]
        // public async Task<IActionResult> AddTutorPersonalInformation(TutorPersonalInformationSaveRequestDTO personalInfoRequestDTO)
        // {

        //     //Requirement for phone number and country code:
        //     //Phone number is null and country code is null - Correct
        //     //Phone number is not null and country code is not null - Correct
        //     //Phone number is null and country code is not null - Incorrect
        //     //Phone number is not null and country code is null - Incorrect
        //     //Check if the above requirements are met
        //     Console.WriteLine("Is country code empty or null: " + string.IsNullOrEmpty(personalInfoRequestDTO.PhoneNumberCountryCode));
        //     Console.WriteLine("Is phone number empty or null: " + string.IsNullOrEmpty(personalInfoRequestDTO.PhoneNumber));
        //     if (string.IsNullOrEmpty(personalInfoRequestDTO.PhoneNumber) || string.IsNullOrEmpty(personalInfoRequestDTO.PhoneNumberCountryCode))
        //     {
        //         return BadRequest(new
        //         {
        //             success = "false",
        //             message = "Phone number and country code must be both provided, or both left empty",
        //             data = new { },
        //             timestamp = DateTime.Now
        //         });

        //     }
        //     //Check if the given email exists
        //     var personEmail = await _personRepository.GetPersonEmailByEmail(personalInfoRequestDTO.TutorEmail);
        //     if (personEmail == null)
        //     {
        //         return NotFound(new
        //         {
        //             success = "false",
        //             message = "Email does not exist",
        //             data = new { },
        //             timestamp = DateTime.Now
        //         });
        //     }


        //     //Check if the given email is already verified
        //     var personEmailVerification = await _personRepository.GetPersonVerificationCodeByEmail(personEmail.Email);
        //     if (personEmailVerification == null || personEmailVerification.IsVerified == false)
        //     {
        //         return BadRequest(new
        //         {
        //             success = "false",
        //             message = "Please verify your email first",
        //             data = new { },
        //             timestamp = DateTime.Now
        //         });
        //     }

        //     //Check if the personal information is already saved

        //     var existingTutorPersonalInformation = await _personRepository.GetTutorPersonInformationByPersonId(personEmail.PersonId);


        //     if (existingTutorPersonalInformation.PersonDetailsId != Guid.Empty)
        //     {
        //         return BadRequest(new
        //         {
        //             success = "false",
        //             message = "Personal information is already saved for this account",
        //             data = new
        //             {
        //             },
        //             timestamp = DateTime.Now
        //         });
        //     }
        //     //Check if the username is already taken

        //     var existingUsername = await _personRepository.GetTutorByUsername(personalInfoRequestDTO.Username);

        //     if (existingUsername != null)
        //     {
        //         return BadRequest(new
        //         {
        //             success = "false",
        //             message = "Username is already taken",
        //             data = new
        //             {
        //             },
        //             timestamp = DateTime.Now
        //         });

        //     }

        //     //Check if the country of origin is valid
        //     if (!string.IsNullOrEmpty(personalInfoRequestDTO.CountryOfOrigin))
        //     {
        //         var country = await _countryRepository.GetCountryByOfficialNameOrName(personalInfoRequestDTO.CountryOfOrigin);

        //         if (country == null)
        //         {
        //             return BadRequest(new
        //             {
        //                 success = "false",
        //                 message = "Country of origin does not exist",
        //                 data = new
        //                 {
        //                 },
        //                 timestamp = DateTime.Now
        //             });

        //         }
        //     }

        //     //Check if the national calling code is valid 
        //     if (!string.IsNullOrEmpty(personalInfoRequestDTO.PhoneNumberCountryCode))
        //     {
        //         var countryCallingCode = await _countryRepository.GetCountryByNationalCallingCode(personalInfoRequestDTO.PhoneNumberCountryCode);

        //         if (countryCallingCode == null)
        //         {
        //             return BadRequest(new
        //             {
        //                 success = "false",
        //                 message = "National calling code does not exist",
        //                 data = new
        //                 {
        //                 },
        //                 timestamp = DateTime.Now
        //             });
        //         }
        //     }

        //     //Create new PersonDetails object 
        //     var newPersonDetails = new PersonDetails
        //     {
        //         PersonDetailsId = Guid.NewGuid(),
        //         PersonId = personEmail.PersonId,
        //         Person = personEmail.Person,
        //         FirstName = personalInfoRequestDTO.FirstName,
        //         LastName = personalInfoRequestDTO.LastName,
        //         Username = personalInfoRequestDTO.Username,
        //         PhoneNumberCountryCode = personalInfoRequestDTO.PhoneNumberCountryCode,
        //         PhoneNumber = personalInfoRequestDTO.PhoneNumber,
        //         CountryOfOrigin = personalInfoRequestDTO.CountryOfOrigin,
        //         CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
        //         ModifiedAt = null,
        //     };

        //     //Save new PersonDetails object to the database
        //     var addingToDatabaseResult = await _personRepository.CreateNewPersonDetails(newPersonDetails);

        //     if (addingToDatabaseResult == null)
        //     {
        //         return StatusCode(500, new
        //         {
        //             success = "false",
        //             message = "Personal information could not be saved, please try again later",
        //             data = new { },
        //             timestamp = DateTime.Now
        //         });
        //     }
        //     //Configure return object
        //     var returnObjectDTO = new PersonSavePersonDetailsResponseDTO
        //     {
        //         FirstName = newPersonDetails.FirstName,
        //         LastName = newPersonDetails.LastName,
        //         Username = newPersonDetails.Username,
        //         PhoneNumberCountryCode = newPersonDetails.PhoneNumberCountryCode,
        //         PhoneNumber = newPersonDetails.PhoneNumber,
        //         CountryOfOrigin = newPersonDetails.CountryOfOrigin,


        //     };

        //     return Ok(new
        //     {
        //         success = "true",
        //         message = "Personal information has been saved",
        //         data = new
        //         {
        //             PersonalInformation = returnObjectDTO,
        //         },
        //         timestamp = DateTime.Now
        //     });


        // }

        [HttpPut("signup/status")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> UpdateTutorRegistrationStatus(TutorRegistrationStatusUpdateRequestDTO updateRequestDTO)
        {

            Console.WriteLine("Tutor registration status to update:", updateRequestDTO.tutorRegistrationStatusId);

            if (updateRequestDTO.tutorRegistrationStatusId == null)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Tutor registration status is required",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }
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

            //Check if the the tutorId from the updateRequestDTO matches the tutorId associated with the personId from the database
            var tutor = await _tutorRepository.GetTutorByPersonId(personId);


            if (tutor == null)
            {
                return Unauthorized(
                    new
                    {
                        success = "false",
                        message = "You are not a tutor",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the status to update is a valid one
            var newTutorRegistrationStatus = await _referenceRepository.GetTutorRegistrationStatusByIdAsync(updateRequestDTO.tutorRegistrationStatusId);

            if (newTutorRegistrationStatus == null)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Invalid tutor registration status",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the tutorRegistrationStatusId is below or equal to the current tutorRegistrationStatusId in the Tutor table 
            if (updateRequestDTO.tutorRegistrationStatusId <= tutor.TutorRegistrationStatusId)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "You cannot update the tutor registration status to a status that is lower than or equal to the current status",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Update the tutor registration status
            var updateResult = await _tutorRepository.UpdateTutorRegistrationStatus(personId, updateRequestDTO.tutorRegistrationStatusId);

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

            var tutorWithNewStatus = await _tutorRepository.GetTutorByPersonId(personId);

            return Ok(
                new
                {
                    success = "true",
                    message = "Tutor registration status updated successfully",
                    data = new
                    {
                        tutorId = tutorWithNewStatus.TutorId,
                        tutorRegistrationStatusId = tutorWithNewStatus.TutorRegistrationStatusId,

                    },
                    timestamp = DateTime.Now
                }
            );


        }
    }
}