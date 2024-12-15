using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person.PersonDetails;
using backend.DTOs.Person.PersonPhoneNumber;
using backend.Entities.Reference.Country;
using backend.Interfaces.Person;
using backend.Interfaces.Reference;
using backend.Interfaces.Tutor;
using backend.Middleware;
using backend.Middleware.Tutor;
using backend.Repositories.Person;
using backend.Repositories.Tutor;
using EduConnect.Entities.Person;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace backend.Controllers.Person
{
    [ApiController]

    [Route("person/details")]
    [CheckPersonLoginSignup]

    public class PersonDetailsController(IPersonRepository _personRepository, ITutorRepository _tutorRepository, ICountryRepository _countryRepository) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreatePersonDetails(PersonDetailsSaveRequestDTO saveRequestDTO)
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
                var personEmail = await _personRepository.GetPersonEmailByEmail(email);
                personId = personEmail.PersonId;
            }


            //Check if the PersonId is Tutor and if it is, check the TutorRegistrationStatus

            var tutor = await _tutorRepository.GetTutorRegistrationStatusByPersonId(personId);
            Console.WriteLine("Is person a tutor: " + tutor != null);
            //If tutor is not null, check the TutorRegistrationStatus is below 2 (Email Verification, status before)
            if (tutor != null && tutor.TutorRegistrationStatusId < 3)
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

            //Check if the country of origin is valid
            Country countryOfOrigin = null;
            if (saveRequestDTO.CountryOfOriginCountryId.HasValue && saveRequestDTO.CountryOfOriginCountryId != Guid.Empty)
            {
                countryOfOrigin = await _countryRepository.GetCountryById(saveRequestDTO.CountryOfOriginCountryId.Value);

                if (countryOfOrigin == null)
                {
                    return BadRequest(new
                    {
                        success = "false",
                        message = "Country of origin does not exist",
                        data = new
                        {
                        },
                        timestamp = DateTime.Now
                    });

                }
            }


            //Check if the PersonId from personEmail already exists in the database PersonDetails table
            var existingPersonDetails = await _personRepository.GetPersonDetailsByPersonId(personId);
            if (existingPersonDetails != null)
            {
                return Conflict(
                    new
                    {
                        success = "false",
                        message = "This account already has details",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the username and if not null is it already taken
            var existingUsername = await _personRepository.GetPersonByUsername(saveRequestDTO.Username);

            if (existingUsername != null)
            {
                return Conflict(
                    new
                    {
                        success = "false",
                        message = "Username is already taken",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }


            //Check if the person is a tutor, and update their TUtorRegistrationStatusId to 3 
            if (tutor != null)
            {
                var updatedTutorRegistrationStatus = await _tutorRepository.UpdateTutorRegistrationStatus(personId, 4);
                if (updatedTutorRegistrationStatus == null)
                {
                    return BadRequest(
                        new
                        {
                            success = "false",
                            message = "Failed to update tutor registration status",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }
            }

            //Create new PersonDetails object
            PersonDetails newPersonDetails = new PersonDetails
            {
                PersonDetailsId = Guid.NewGuid(),
                PersonId = personId,
                FirstName = saveRequestDTO.FirstName,
                LastName = saveRequestDTO.LastName,
                Username = saveRequestDTO.Username,
                CountryOfOriginCountryId = countryOfOrigin?.CountryId,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            };

            //Create new PersonPhoneNumber object
            PersonPhoneNumberSaveDTO? personPhoneNumberDTO = null;
            PersonPhoneNumberDTO createdPersonPhoneNumber = null;




            var saveResult = await _personRepository.CreateNewPersonDetails(newPersonDetails);


            if (saveResult == null)
            {

                //If the update failed, and the person is a tutor update their TUtorRegistrationStatusId to 2
                if (tutor != null)
                {
                    var updatedTutorRegistrationStatus = await _tutorRepository.UpdateTutorRegistrationStatus(personId, 3);
                }
                return StatusCode(
                    500,
                    new
                    {
                        success = "error",
                        message = "We failed to save person details, please try again later",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }



            return Ok(
                new
                {
                    success = "true",
                    message = "Person details saved successfully",
                    data = new
                    {
                        personDetails = new
                        {
                            personDetailsId = saveResult.PersonDetailsId,
                            firstName = saveResult.FirstName,
                            lastName = saveResult.LastName,
                            username = saveResult.Username,
                            countryOfOriginCountryId = saveResult.CountryOfOriginCountryId,
                            countryOfOriginCountryName = countryOfOrigin?.Name,
                        }
                    },
                    timestamp = DateTime.Now
                }
            );


        }

        [HttpGet]
        public async Task<IActionResult> GetPersonDetails()
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
                var personEmail = await _personRepository.GetPersonEmailByEmail(email);
                personId = personEmail.PersonId;
            }

            //Check if the PersonId is Tutor and if it is, check the TutorRegistrationStatus

            var tutor = await _tutorRepository.GetTutorRegistrationStatusByPersonId(personId);
            Console.WriteLine("Tutor Id: " + tutor.PersonId);
            //If tutor is not null, check the TutorRegistrationStatus is below 3 (Email Verification, status before)
            if (tutor != null && tutor.TutorRegistrationStatusId < 3)
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

            //Get the PersonDetails from the database 
            var personDetails = await _personRepository.GetPersonDetailsByPersonId(personId);
            if (personDetails == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Person details for this account not found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Get the Country data for the CountryOfOriginCountryId
            Country countryOfOrigin = null;
            if (personDetails.CountryOfOriginCountryId != Guid.Empty || personDetails.CountryOfOriginCountryId.HasValue)
            {

                countryOfOrigin = await _countryRepository.GetCountryById(personDetails.CountryOfOriginCountryId.Value);

            }

            //Get the PersonPhoneNumber from the database
            var personPhoneNumber = await _personRepository.GetPersonPhoneNumberByPersonId(personId);


            //Get the Country data for the NationalCallingCodeCountryId
            Country countryByCallingCode = null;
            if (personPhoneNumber != null)
            {
                countryByCallingCode = await _countryRepository.GetCountryById(personPhoneNumber.NationalCallingCodeCountryId);
            }
            return Ok(
                new
                {
                    success = "true",
                    message = "Person details retrieved successfully",
                    data = new
                    {
                        PersonDetails = new
                        {
                            PersonDetailsId = personDetails.PersonDetailsId,
                            FirstName = personDetails.FirstName,
                            LastName = personDetails.LastName,
                            Username = personDetails.Username,
                            CountryOfOriginCountryId = personDetails.CountryOfOriginCountryId,
                            CountryOfOriginCountryName = countryOfOrigin?.Name
                        },
                        PersonPhoneNumber = new
                        {
                            PhoneNumberId = personPhoneNumber?.PersonPhoneNumberId,
                            NationalCallingCodeCountryId = personPhoneNumber?.NationalCallingCodeCountryId,
                            NationalCallingCodeCountryName = countryByCallingCode?.Name,
                            PhoneNumber = personPhoneNumber?.PhoneNumber
                        }
                    },
                    timestamp = DateTime.Now
                }
            );

        }


    }


}