using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person.PersonDetails;
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
            //Requirement for phone number and country code:
            //Phone number is null and country code is null - Correct
            //Phone number is not null and country code is not null - Correct
            //Phone number is null and country code is not null - Incorrect
            //Phone number is not null and country code is null - Incorrect
            //Check if the above requirements are met
            Console.WriteLine("Is country code empty or null: " + string.IsNullOrEmpty(saveRequestDTO.PhoneNumberCountryCode));
            Console.WriteLine("Is phone number empty or null: " + string.IsNullOrEmpty(saveRequestDTO.PhoneNumber));
            if (string.IsNullOrEmpty(saveRequestDTO.PhoneNumber) || string.IsNullOrEmpty(saveRequestDTO.PhoneNumberCountryCode))
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "Phone number and country code must be both provided, or both left empty",
                    data = new { },
                    timestamp = DateTime.Now
                });

            }

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
            Console.WriteLine("Tutor Id: " + tutor.PersonId);
            //If tutor is not null, check the TutorRegistrationStatus is below 2 (Email Verification, status before)
            if (tutor != null && tutor.TutorRegistrationStatusId < 2)
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
            if (!string.IsNullOrEmpty(saveRequestDTO.CountryOfOrigin))
            {
                var country = await _countryRepository.GetCountryByOfficialNameOrName(saveRequestDTO.CountryOfOrigin);

                if (country == null)
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

            //Check if the national calling code is valid 
            if (!string.IsNullOrEmpty(saveRequestDTO.PhoneNumberCountryCode))
            {
                var countryCallingCode = await _countryRepository.GetCountryByNationalCallingCode(saveRequestDTO.PhoneNumberCountryCode);

                if (countryCallingCode == null)
                {
                    return BadRequest(new
                    {
                        success = "false",
                        message = "National calling code does not exist",
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

            //Check if the username and if not null phone number is already taken
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

            //Check if the phone number if not null, is already taken
            if (!string.IsNullOrEmpty(saveRequestDTO.PhoneNumber))
            {

                var existingPhoneNumber = await _personRepository.GetPersonByPhoneNumber(saveRequestDTO.PhoneNumberCountryCode, saveRequestDTO.PhoneNumber);
                if (existingPhoneNumber != null)
                {
                    return Conflict(
                        new
                        {
                            success = "false",
                            message = "Phone number is already taken",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }
            }
            //Save the person details to the database
            PersonDetails newPersonDetails = new PersonDetails
            {
                PersonDetailsId = Guid.NewGuid(),
                PersonId = personId,
                FirstName = saveRequestDTO.FirstName,
                LastName = saveRequestDTO.LastName,
                Username = saveRequestDTO.Username,
                PhoneNumberCountryCode = saveRequestDTO.PhoneNumberCountryCode,
                PhoneNumber = saveRequestDTO.PhoneNumber,
                CountryOfOrigin = saveRequestDTO.CountryOfOrigin,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            };

            var saveResult = await _personRepository.CreateNewPersonDetails(newPersonDetails);


            if (saveResult == null)
            {
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
                saveResult
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
            //If tutor is not null, check the TutorRegistrationStatus is below 2 (Email Verification, status before)
            if (tutor != null && tutor.TutorRegistrationStatusId < 2)
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
                            PhoneNumberCountryCode = personDetails.PhoneNumberCountryCode,
                            PhoneNumber = personDetails.PhoneNumber,
                            CountryOfOrigin = personDetails.CountryOfOrigin,
                        }
                    },
                    timestamp = DateTime.Now
                }
            );

        }

        [HttpPut]
        public async Task<IActionResult> UpdatePersonDetails(PersonDetailsUpdateRequestDTO updateRequestDTO)
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
            //If tutor is not null, check the TutorRegistrationStatus is below 2 (Email Verification, status before)
            if (tutor != null && tutor.TutorRegistrationStatusId < 2)
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

            //Data validation from request, check if any of the fields are changed 

            //Flag isUpdated to keep track of if any of the fields are changed
            bool isUpdated = false;

            //Check if FirstName is changed
            if (!string.IsNullOrEmpty(updateRequestDTO.FirstName) && personDetails.FirstName != updateRequestDTO.FirstName)
            {
                personDetails.FirstName = updateRequestDTO.FirstName;
                isUpdated = true;
            }

            //Check if LastName is changed
            if (!string.IsNullOrEmpty(updateRequestDTO.LastName) && personDetails.LastName != updateRequestDTO.LastName)
            {
                personDetails.LastName = updateRequestDTO.LastName;
                isUpdated = true;
            }

            //Check if Username is changed and is taken
            if (!string.IsNullOrEmpty(updateRequestDTO.Username))
            {

                //Check if the username is taken
                var personUsername = await _personRepository.GetPersonByUsername(updateRequestDTO.Username);
                if (personUsername != null)
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

                if (personDetails.Username != updateRequestDTO.Username)
                {
                    personDetails.Username = updateRequestDTO.Username;
                    isUpdated = true;
                }
            }



            //Check if the national calling code is valid 
            if (!string.IsNullOrEmpty(updateRequestDTO.PhoneNumberCountryCode))
            {
                var countryCallingCode = await _countryRepository.GetCountryByNationalCallingCode(updateRequestDTO.PhoneNumberCountryCode);

                if (countryCallingCode == null)
                {
                    return BadRequest(new
                    {
                        success = "false",
                        message = "National calling code does not exist",
                        data = new
                        {
                        },
                        timestamp = DateTime.Now
                    });
                }

                //Check if the PhoneNumberCountryCode has changed
                if (personDetails.PhoneNumberCountryCode != updateRequestDTO.PhoneNumberCountryCode)
                {
                    personDetails.PhoneNumberCountryCode = updateRequestDTO.PhoneNumberCountryCode;
                    isUpdated = true;
                }
            }

            //Check phone number
            //1. Check if the changed country code and changed phone number already exist in the database
            if (!string.IsNullOrEmpty(updateRequestDTO.PhoneNumberCountryCode) && !string.IsNullOrEmpty(updateRequestDTO.PhoneNumber))
            {
                var existingPhoneNumberRecord = await _personRepository.GetPersonByPhoneNumber(updateRequestDTO.PhoneNumberCountryCode, updateRequestDTO.PhoneNumber);

                if (existingPhoneNumberRecord != null)
                {
                    return Conflict(
                        new
                        {
                            success = "false",
                            message = "Phone number already exists",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }
            }

            //2. Check if the existing phone number with changed country code already exist in the database
            if (!string.IsNullOrEmpty(updateRequestDTO.PhoneNumberCountryCode) && !string.IsNullOrEmpty(personDetails.PhoneNumber))
            {
                var existingPhoneNumberRecord = await _personRepository.GetPersonByPhoneNumber(updateRequestDTO.PhoneNumberCountryCode, personDetails.PhoneNumber);

                if (existingPhoneNumberRecord != null)
                {
                    return Conflict(
                        new
                        {
                            success = "false",
                            message = "Phone number already exists",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }

            }


            //3. Check if the changed phone number with existing country code already exist in the database
            if (!string.IsNullOrEmpty(personDetails.PhoneNumberCountryCode) && !string.IsNullOrEmpty(updateRequestDTO.PhoneNumber))
            {
                return Conflict(
                    new
                    {
                        success = "false",
                        message = "Phone number already exists",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the country code has changed
            if (!string.IsNullOrEmpty(updateRequestDTO.PhoneNumberCountryCode) && updateRequestDTO.PhoneNumberCountryCode != personDetails.PhoneNumberCountryCode)
            {
                personDetails.PhoneNumberCountryCode = updateRequestDTO.PhoneNumberCountryCode;
                isUpdated = true;
            }

            //Check if the phone number has changed
            if (!string.IsNullOrEmpty(updateRequestDTO.PhoneNumber) && updateRequestDTO.PhoneNumber != personDetails.PhoneNumber)
            {
                personDetails.PhoneNumber = updateRequestDTO.PhoneNumber;
                isUpdated = true;
            }


            //Check if the CountryOfOrigin has changed, if it has changed, then check if the country exists
            Console.WriteLine("CountryOfOrigin: " + updateRequestDTO.CountryOfOrigin);
            if (!string.IsNullOrEmpty(updateRequestDTO.CountryOfOrigin) && updateRequestDTO.CountryOfOrigin != personDetails.CountryOfOrigin)
            {
                var country = await _countryRepository.GetCountryByOfficialNameOrName(updateRequestDTO.CountryOfOrigin);
                if (country == null)
                {
                    return NotFound(new
                    {
                        success = "false",
                        message = "Country of origin does not exist",
                        data = new
                        {
                        },
                        timestamp = DateTime.Now
                    });
                }

                personDetails.CountryOfOrigin = updateRequestDTO.CountryOfOrigin;
                isUpdated = true;
            }

            //Check if no changes happened
            if (!isUpdated)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "No new values to update were provided",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Convert the person details to a DTO
            var personDetailsDTO = new PersonDetailsUpdateDTO
            {
                PersonDetailsId = personDetails.PersonDetailsId,
                PersonId = personDetails.PersonId,
                FirstName = personDetails.FirstName,
                LastName = personDetails.LastName,
                Username = personDetails.Username,
                PhoneNumberCountryCode = personDetails.PhoneNumberCountryCode,
                PhoneNumber = personDetails.PhoneNumber,
                CountryOfOrigin = personDetails.CountryOfOrigin
            };
            //Update the person details 
            var updatedPersonDetails = await _personRepository.UpdatePersonDetails(personDetailsDTO);

            if (updatedPersonDetails == null)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "false",
                        message = "We failed to update data, please try again later",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            return Ok(
                new
                {
                    success = "true",
                    message = "Person details updated successfully",
                    data = new
                    {
                        personDetails = new
                        {
                            personDetailsId = updatedPersonDetails.PersonDetailsId,
                            firstName = updatedPersonDetails.FirstName,
                            lastName = updatedPersonDetails.LastName,
                            username = updatedPersonDetails.Username,
                            phoneNumberCountryCode = updatedPersonDetails.PhoneNumberCountryCode,
                            phoneNumber = updatedPersonDetails.PhoneNumber,
                            countryOfOrigin = updatedPersonDetails.CountryOfOrigin
                        }
                    },
                    timestamp = DateTime.Now
                }
            );



        }

    }


}