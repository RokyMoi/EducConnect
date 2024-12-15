using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person.PersonPhoneNumber;
using backend.Interfaces.Person;
using backend.Interfaces.Reference;
using backend.Interfaces.Tutor;
using backend.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Person
{
    [ApiController]
    [Route("person/phone-number")]
    [CheckPersonLoginSignup]
    public class PersonPhoneNumberController(IPersonRepository _personRepository, IPersonPhoneNumberRepository _personPhoneNumberRepository, ITutorRepository _tutorRepository, ICountryRepository _countryRepository) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreatePersonPhoneNumber(PersonPhoneNumberSaveRequestDTO saveRequestDTO)
        {

            //Data validation for the saveRequestDTO
            //Check the saveRequest NationalCallingCodeCountryId is valid (not equal to empty guid)
            if (saveRequestDTO.NationalCallingCodeCountryId == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "National calling code country id is required",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }
            //Check if the saveRequest PhoneNumber field is valid
            if (string.IsNullOrEmpty(saveRequestDTO.PhoneNumber))
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Phone number is required",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
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
            //If tutor is not null, check the TutorRegistrationStatus is below 4 (Personal Information, status before)
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

            //Check if the PersonPhoneNumber table contains the PersonId
            var personPhoneNumber = await _personPhoneNumberRepository.GetPersonPhoneNumberByPersonId(personId);
            if (personPhoneNumber != null)
            {
                return Conflict(
                    new
                    {
                        success = "false",
                        message = "Phone number has been already added for this account",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }

            //Check if the saveRequest NationalCallingCodeCountryId exists in the database
            var countryByCallingCode = await _countryRepository.GetCountryById(saveRequestDTO.NationalCallingCodeCountryId);

            if (countryByCallingCode == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Provided national calling does not exist",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the saveRequest PhoneNumber with the given NationalCallingCodeCountryId exists in the database
            var personPhoneNumberByPhoneNumber = await _personPhoneNumberRepository.GetPersonPhoneNumberByPhoneNumber(saveRequestDTO.PhoneNumber, saveRequestDTO.NationalCallingCodeCountryId);
            if (personPhoneNumberByPhoneNumber != null)
            {
                return Conflict(
                    new
                    {
                        success = "false",
                        message = "Phone number is taken",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //If person is tutor, update the TutorRegistrationStatus to 3 (Phone number)
            if (tutor != null)
            {
                var updateTutorRegistrationStatusResult = await _tutorRepository.UpdateTutorRegistrationStatus(tutor.PersonId, 3);
                if (updateTutorRegistrationStatusResult == null)
                {
                    return StatusCode(
                        500,
                        new
                        {
                            success = "false",
                            message = "We failed to save the data, please try again later",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }
            }

            //Attempt to save the PersonPhoneNumber
            var personPhoneNumberSaveDTO = new PersonPhoneNumberSaveDTO
            {
                PersonId = personId,
                PhoneNumber = saveRequestDTO.PhoneNumber,
                NationalCallingCodeCountryId = saveRequestDTO.NationalCallingCodeCountryId,
            };

            var saveResult = await _personPhoneNumberRepository.CreatePersonPhoneNumber(personPhoneNumberSaveDTO);

            if (saveResult == null)
            {
                //If the saveResult failed, update the TutorRegistrationStatus to 2 (Personal Information)
                await _tutorRepository.UpdateTutorRegistrationStatus(tutor.PersonId, 2);
                if (tutor != null)
                    return StatusCode(
                        500,
                        new
                        {
                            success = "false",
                            message = "We failed to save the data, please try again later",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
            }



            return Ok(new
            {
                success = "true",
                message = "Person phone number created successfully",
                data = new
                {
                    phoneNumber = new
                    {
                        PersonPhoneNumberId = saveResult.PersonPhoneNumberId,
                        NationalCallingCode = countryByCallingCode.NationalCallingCode,
                        NationalCallingCodeCountryName = countryByCallingCode.Name,
                        PhoneNumber = saveResult.PhoneNumber,



                    }
                },
                timestamp = DateTime.Now
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePersonPhoneNumber(PersonPhoneNumberUpdateRequestDTO updateRequestDTO)
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
            Console.WriteLine("Tutor Id: " + tutor.PersonId);
            //If tutor is not null, check the TutorRegistrationStatus is below 4 (Personal Information, status before)
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


            //Check if the PersonPhoneNumber contains the PersonId
            var personPhoneNumber = await _personPhoneNumberRepository.GetPersonPhoneNumberByPersonId(personId);

            if (personPhoneNumber == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Phone number for this account not found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Data validation
            //Set flag isUpdated to track update status
            bool isUpdated = false;

            //Check if the PhoneNumber is null or empty, and if is updated value
            if (!string.IsNullOrEmpty(updateRequestDTO.PhoneNumber) && personPhoneNumber.PhoneNumber != updateRequestDTO.PhoneNumber)
            {
                personPhoneNumber.PhoneNumber = updateRequestDTO.PhoneNumber;
                isUpdated = true;
            }

            //Check if the NationalCallingCode is null or empty or empty guid, and if is updated value
            if (updateRequestDTO.NationalCallingCodeCountryId.HasValue && personPhoneNumber.NationalCallingCodeCountryId != updateRequestDTO.NationalCallingCodeCountryId)
            {
                //Check if the NationalCallingCodeCountryId is valid
                var country = await _countryRepository.GetCountryById(updateRequestDTO.NationalCallingCodeCountryId.Value);

                if (country == null)
                {
                    return NotFound(
                        new
                        {
                            success = "false",
                            message = "National calling code does not exist",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }


                personPhoneNumber.NationalCallingCodeCountryId = updateRequestDTO.NationalCallingCodeCountryId.Value;
                isUpdated = true;
            }


            //Check if any of the fields are updated
            if (!isUpdated)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "No new values were provided to update",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the updated phone number is already in use
            var personPhoneNumberByPhoneNumber = await _personPhoneNumberRepository.GetPersonPhoneNumberByPhoneNumber(personPhoneNumber.PhoneNumber, personPhoneNumber.NationalCallingCodeCountryId);

            if (personPhoneNumberByPhoneNumber != null)
            {
                return Conflict(
                    new
                    {
                        success = "false",
                        message = "Phone number is taken",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Attempt to update the person phone number
            var updateResult = await _personPhoneNumberRepository.UpdatePersonPhoneNumber(personPhoneNumber);
            if (updateResult == null)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "false",
                        message = "We failed to update your phone number. Please try again later",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }
            var countryByCallingCode = await _countryRepository.GetCountryById(updateResult.NationalCallingCodeCountryId);
            return Ok(new
            {
                success = "true",
                message = "Person phone number updated successfully",
                data = new
                {
                    personPhoneNumber = new
                    {
                        PersonPhoneNumberId = personPhoneNumber.PersonPhoneNumberId,
                        NationalCallingCodeCountryId = personPhoneNumber.NationalCallingCodeCountryId,
                        NationalCallingCodeCountryName = countryByCallingCode.Name,
                        NationalCallingCode = countryByCallingCode.NationalCallingCode,
                        PhoneNumber = personPhoneNumber.PhoneNumber,


                    }
                },
                timestamp = DateTime.Now
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePersonPhoneNumber()
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
            Console.WriteLine("Tutor Id: " + tutor.PersonId);
            //If tutor is not null, check the TutorRegistrationStatus is below 4 (Personal Information, status before)
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


            //Check if the PersonPhoneNumber contains the PersonId
            var personPhoneNumber = await _personPhoneNumberRepository.GetPersonPhoneNumberByPersonId(personId);

            if (personPhoneNumber == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Phone number for this account not found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Delete the PersonPhoneNumber
            var deleteResult = await _personPhoneNumberRepository.DeletePersonPhoneNumberByPersonId(personId);
            if (deleteResult == null)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "false",
                        message = "We failed to delete your phone number. Please try again later",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }
            return Ok(new
            {
                success = "true",
                message = "Person phone number deleted successfully",
                data = new { },
                timestamp = DateTime.Now
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonPhoneNumber()
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
            Console.WriteLine("Tutor Id: " + tutor.PersonId);
            //If tutor is not null, check the TutorRegistrationStatus is below 4 (Personal Information, status before)
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


            //Check if the PersonPhoneNumber contains the PersonId
            var personPhoneNumber = await _personPhoneNumberRepository.GetPersonPhoneNumberByPersonId(personId);

            if (personPhoneNumber == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Phone number for this account not found",
                        data = new
                        {
                        },
                        timestamp = DateTime.Now
                    }
                );
            }


            return Ok(new
            {
                success = "true",
                message = "Person phone number retrieved successfully",
                data = new
                {
                    PersonPhoneNumberId = personPhoneNumber.PersonPhoneNumberId,
                    NationalCallingCodeCountryId = personPhoneNumber.NationalCallingCodeCountryId,
                    NationalCallingCodeCountryName = personPhoneNumber.NationalCallingCodeCountryName,
                    NationalCallingCode = personPhoneNumber.NationalCallingCode,
                    PhoneNumber = personPhoneNumber.PhoneNumber,
                },
                timestamp = DateTime.Now
            });
        }


    }
}