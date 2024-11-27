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
using backend.Services;
using backend.Entities.Person;
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

            var existingEmail = await _personRepository.GetPersonEmailByEmail(tutorSingupRequest.Email);

            //Check is email taken
            if (existingEmail != null)
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "Email already taken",
                    data = new { },
                    timestamp = DateTime.Now

                });
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


            //Create new PersonVerificationCode
            var PersonVerificationCode = new PersonVerificationCode
            {
                PersonVerificationCodeId = Guid.NewGuid(),
                PersonId = Person.PersonId,
                Person = Person,
                VerificationCode = EncryptionUtilities.GenerateRandomString(),
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
                    PersonSalt,
                    PersonVerificationCode
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


            //Template for successful signup to platform as Tutor
            /*
            Dear [Tutor's Name],

Congratulations on joining EduConnect! We are thrilled to welcome you as part of our global community of knowledge sharers.

As a tutor on EduConnect, you have the opportunity to make a meaningful impact by sharing your expertise with eager learners worldwide. We're excited to see the amazing courses and lessons you will create to inspire and educate others.

Before you can fully explore the platform and start sharing your knowledge, we need to verify your email address. This is an important step to secure your account and ensure smooth communication.

Please verify your email by clicking the link below:
[Verification Link]

If you didn’t sign up for EduConnect, please ignore this email.

Thank you for choosing EduConnect as your platform to spread knowledge. Together, we’re shaping the future of education.

Best regards,  
The EduConnect Team  

P.S. Need help or have questions? Feel free to reach out to us at support@educonnect.com.  

            */
            //Attempt to send email to given email address
            var emailResult = await EmailService.SendEmailToAsync(tutorSingupRequest.Email, "Welcome to EduConnect!",

                    "Hello, \n\n"
                    + "Congratulations on joining EduConnect! We are thrilled to welcome you as part of our global community of knowledge sharers.\n\n"
                    + "As a tutor on EduConnect, you have the opportunity to make a meaningful impact by sharing your expertise with eager learners worldwide. We're excited to see the amazing courses and lessons you will create to inspire and educate others.\n\n"
                    + "Before you can fully explore the platform and start sharing your knowledge, we need to verify your email address. This is an important step to secure your account and ensure smooth communication.\n\n"
                    + $"Your verification code is: {savedPersonDataDTO.PersonVerificationCodeDTO.VerificationCode}\n\n"
                    + $"Notice: This code expires at {DateTimeOffset.FromUnixTimeMilliseconds(savedPersonDataDTO.PersonVerificationCodeDTO.ExpiryDateTime).ToUniversalTime().ToString("HH:mm:ss dd.MM.yyyy.  'UTC'")}\n\n"
                    + "Please use this code to verify your email within the platform.\n\n"
                    + "If you didn’t sign up for EduConnect, please ignore this email.\n\n"
                    + "Thank you for choosing EduConnect as your platform to spread knowledge. Together, we’re shaping the future of education.\n\n"
                    + "Best regards,\n"
                    + "The EduConnect Team\n\n"
                    + "P.S. Need help or have questions? Feel free to reach out to us at support@educonnect.com.\n"
            );

            if (!emailResult)
            {
                return BadRequest("Email address is not a registered email address");
            }


            return Ok(new
            {
                message = "You have successfully registered as a tutor on EduConnect, please verify your email address using the verification code sent to your email address",
                data = tutorSignupResponseDTO
            });

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
            if (verificationCodeFromDatabase.IsVerified) { 
                return BadRequest(new
                {
                    success = "false",
                    message = "User with given email address is already verified",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Check is the existing verification code expired
            if (verificationCodeFromDatabase.ExpiryDateTime > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) { 
                return BadRequest(new { 
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

    }
}