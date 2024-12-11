using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person;
using backend.DTOs.Person.PersonAvailability;
using backend.Interfaces.Person;
using backend.Interfaces.Tutor;
using backend.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Person
{
    [ApiController]
    [Route("person/availability")]
    [CheckPersonLoginSignup]
    public class PersonAvailabilityController(IPersonRepository _personRepository, IPersonAvailabilityRepository _personAvailabilityRepository, ITutorRepository _tutorRepository) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddPersonAvailability(PersonAvailabilitySaveRequestDTO saveRequestDTO)
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
                var personEmailObject = await _personRepository.GetPersonEmailByEmail(email);
                personId = personEmailObject.PersonId;
            }


            //Check if the PersonId is Tutor and if it is, check the TutorRegistrationStatus

            var tutor = await _tutorRepository.GetTutorRegistrationStatusByPersonId(personId);
            Console.WriteLine("Tutor Id: " + tutor.PersonId);
            //If tutor is not null, check the TutorRegistrationStatus is below 5 (Personal Information, status before)
            if (tutor != null && tutor.TutorRegistrationStatusId < 5)
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




            Console.WriteLine("Is day of the week valid: " + Enum.IsDefined(typeof(DayOfWeek), saveRequestDTO.DayOfWeek));
            //Check if the day of the week is valid
            if (!Enum.IsDefined(typeof(DayOfWeek), saveRequestDTO.DayOfWeek))
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Invalid day of the week",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }

            //Check TimeSpan StartTime and EndTime
            TimeSpan startTime;

            if (!TimeSpan.TryParse(saveRequestDTO.StartTime, out startTime))
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Invalid start time",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }
            TimeSpan endTime;
            if (!TimeSpan.TryParse(saveRequestDTO.EndTime, out endTime))
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Invalid end time",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }


            //Check if the start time is before the end time
            if (startTime.CompareTo(endTime) >= 0)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Start time must be before end time",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }

            //Check if the start time and end time difference is less than 15 min
            if (startTime.Subtract(endTime).Duration().Minutes < 15)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Start time and end time difference must be at least 15 minutes",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }


            //Create a new PersonAvailabilityDTO
            var personAvailabilityDTO = new PersonAvailabilityDTO
            {
                PersonId = personId,
                DayOfWeek = (DayOfWeek)saveRequestDTO.DayOfWeek,
                StartTime = startTime,
                EndTime = endTime,

            };


            //If the person is a tutor, update the TutorRegistrationStatus to 4 (Education Information)
            //Check if the person is a tutor, and update their TUtorRegistrationStatusId to 4
            if (tutor != null)
            {
                var updatedTutorRegistrationStatus = await _tutorRepository.UpdateTutorRegistrationStatus(personId, 6);
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

            //Add the PersonAvailabilityDTO to the database
            var addResult = await _personAvailabilityRepository.AddPersonAvailability(personAvailabilityDTO);

            if (addResult == null)
            {
                return StatusCode(500,
                    new
                    {
                        success = "false",
                        message = "We failed to add availability data for this account, please try again later",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }

            return Ok(new
            {
                success = "true",
                message = "Availability added successfully",
                data = new
                {
                    timeAvailability = new PersonAvailabilitySaveResponseDTO
                    {
                        PersonAvailabilityId = addResult.PersonAvailabilityId,
                        DayOfWeek = addResult.DayOfWeek,
                        StartTime = addResult.StartTime,
                        EndTime = addResult.EndTime,

                    }
                },
                timestamp = DateTime.Now,
            });

        }

        [HttpGet]
        public async Task<IActionResult> GetPersonAvailabilityById([FromQuery] Guid id)
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
                var personEmailObject = await _personRepository.GetPersonEmailByEmail(email);
                personId = personEmailObject.PersonId;
            }


            //Check if the PersonId is Tutor and if it is, check the TutorRegistrationStatus

            var tutor = await _tutorRepository.GetTutorRegistrationStatusByPersonId(personId);
            Console.WriteLine("Tutor Id: " + tutor.PersonId);
            //If tutor is not null, check the TutorRegistrationStatus is below 5 (Personal Information, status before)
            if (tutor != null && tutor.TutorRegistrationStatusId < 5)
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




            if (id == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Invalid id",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }

            var personAvailability = await _personAvailabilityRepository.GetPersonAvailabilityById(id);

            if (personAvailability == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Requested time availability not found",
                    data = new { },
                    timestamp = DateTime.Now,
                });

            }

            return Ok(
                new
                {
                    success = "true",
                    message = "Requested time availability found",
                    data = new
                    {
                        timeAvailability = new PersonAvailabilitySaveResponseDTO
                        {

                            PersonAvailabilityId = personAvailability.PersonAvailabilityId,
                            DayOfWeek = personAvailability.DayOfWeek,
                            StartTime = personAvailability.StartTime,
                            EndTime = personAvailability.EndTime,
                        }
                    },
                    timestamp = DateTime.Now,
                }
            );
        }


        [HttpPost("all")]
        public async Task<IActionResult> GetAllPersonAvailabilityByEmail(PersonEmailRequestDTO requestDTO)
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
                var personEmailObject = await _personRepository.GetPersonEmailByEmail(email);
                personId = personEmailObject.PersonId;
            }


            //Check if the PersonId is Tutor and if it is, check the TutorRegistrationStatus

            var tutor = await _tutorRepository.GetTutorRegistrationStatusByPersonId(personId);
            Console.WriteLine("Tutor Id: " + tutor.PersonId);
            //If tutor is not null, check the TutorRegistrationStatus is below 5 (Personal Information, status before)
            if (tutor != null && tutor.TutorRegistrationStatusId < 5)
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



            //Check if a PersonEmail associated with the email from the parameter PersonEmailRequestDTO exists in the database
            var personEmail = await _personRepository.GetPersonEmailByEmail(requestDTO.Email);

            if (personEmail == null)
            {
                return Unauthorized(
                    new
                    {
                        success = "false",
                        message = "Email does not exist",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }


            //Get all PersonAvailabilityDTOs associated with the PersonId from PersonEmail

            var personAvailabilityDTOList = await _personAvailabilityRepository.GetAllPersonAvailabilityByPersonId(personEmail.PersonId);

            if (personAvailabilityDTOList == null || personAvailabilityDTOList.Count == 0)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "No availability data found for this account",
                    data = new { },
                    timestamp = DateTime.Now,
                });

            }

            //Convert the PersonAvailabilityDTOList to a List of PersonAvailabilitySaveResponseDTO

            var personAvailabilitySaveResponseDTOList = new List<PersonAvailabilitySaveResponseDTO>();
            foreach (var personAvailabilityDTO in personAvailabilityDTOList)
            {
                personAvailabilitySaveResponseDTOList.Add(
                    new PersonAvailabilitySaveResponseDTO
                    {
                        PersonAvailabilityId = personAvailabilityDTO.PersonAvailabilityId,
                        DayOfWeek = personAvailabilityDTO.DayOfWeek,
                        StartTime = personAvailabilityDTO.StartTime,
                        EndTime = personAvailabilityDTO.EndTime,

                    }
                );
            }

            return Ok(new
            {
                success = "true",
                message = "Found " + personAvailabilitySaveResponseDTOList.Count + " records of availability data for this account",
                data = new
                {
                    timeAvailability = personAvailabilitySaveResponseDTOList
                },
                timestamp = DateTime.Now,
            });



        }

        [HttpDelete]
        public async Task<IActionResult> DeletePersonAvailabilityById(PersonAvailabilityDeleteRequestDTO requestDTO)
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
                var personEmailObject = await _personRepository.GetPersonEmailByEmail(email);
                personId = personEmailObject.PersonId;
            }


            //Check if the PersonId is Tutor and if it is, check the TutorRegistrationStatus

            var tutor = await _tutorRepository.GetTutorRegistrationStatusByPersonId(personId);
            Console.WriteLine("Tutor Id: " + tutor.PersonId);
            //If tutor is not null, check the TutorRegistrationStatus is below 5 (Personal Information, status before)
            if (tutor != null && tutor.TutorRegistrationStatusId < 5)
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



            //Check if a PersonEmail associated with the email from the parameter PersonEmailRequestDTO exists in the database

            var personEmail = await _personRepository.GetPersonEmailByEmail(requestDTO.Email);

            if (personEmail == null)
            {
                return Unauthorized(
                    new
                    {
                        success = "false",
                        message = "Email does not exist",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }


            if (requestDTO.PersonAvailabilityId == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Invalid id",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }


            //Check if a PersonAvailability associated with the id from the parameter PersonAvailabilityDeleteRequestDTO exists in the database
            var personAvailability = await _personAvailabilityRepository.GetPersonAvailabilityById(requestDTO.PersonAvailabilityId);

            if (personAvailability == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Time availability not found",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }

            if (personAvailability.PersonId != personEmail.PersonId)
            {
                return StatusCode(403,
                    new
                    {
                        success = "false",
                        message = "You do not have permission to delete this time availability",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }


            var deleteResult = await _personAvailabilityRepository.DeletePersonAvailabilityById(requestDTO.PersonAvailabilityId);

            if (deleteResult == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Time availability not found",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }

            return Ok(
                    new
                    {
                        success = "true",
                        message = "Time availability deleted successfully",
                        data = new
                        {
                            deletedTimeAvailability = new PersonAvailabilitySaveResponseDTO
                            {
                                PersonAvailabilityId = deleteResult.PersonAvailabilityId,
                                DayOfWeek = deleteResult.DayOfWeek,
                                StartTime = deleteResult.StartTime,
                                EndTime = deleteResult.EndTime,
                            }
                        },
                        timestamp = DateTime.Now,
                    }
                );



        }
        [HttpPut]
        public async Task<IActionResult> UpdatePersonAvailabilityById(PersonAvailabilityUpdateRequestDTO requestDTO)
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
                var personEmailObject = await _personRepository.GetPersonEmailByEmail(email);
                personId = personEmailObject.PersonId;
            }


            //Check if the PersonId is Tutor and if it is, check the TutorRegistrationStatus

            var tutor = await _tutorRepository.GetTutorRegistrationStatusByPersonId(personId);
            Console.WriteLine("Tutor Id: " + tutor.PersonId);
            //If tutor is not null, check the TutorRegistrationStatus is below 5 (Personal Information, status before)
            if (tutor != null && tutor.TutorRegistrationStatusId < 5)
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



            //Check if the PersonAvailabilityId is valid
            if (requestDTO.PersonAvailabilityId == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Invalid id",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }


            //Check StartTime 
            TimeSpan newStartTime = TimeSpan.Zero;
            if (!string.IsNullOrEmpty(requestDTO.StartTime) && !TimeSpan.TryParse(requestDTO.StartTime, out newStartTime))
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Invalid start time",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }

            //Check EndTime 
            TimeSpan newEndTime = TimeSpan.Zero;
            if (!string.IsNullOrEmpty(requestDTO.EndTime) && !TimeSpan.TryParse(requestDTO.EndTime, out newEndTime))
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Invalid end time",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }



            //Check if the new startTime is before the new endTime
            if (!string.IsNullOrEmpty(requestDTO.StartTime) && !string.IsNullOrEmpty(requestDTO.EndTime) && newStartTime.CompareTo(newEndTime) >= 0)
            {


                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Start time must be before end time",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }


            //Check if the new startTime and the new endTime difference is at least 15 minutes
            if (!string.IsNullOrEmpty(requestDTO.StartTime) && !string.IsNullOrEmpty(requestDTO.EndTime) && newEndTime.Subtract(newStartTime).Duration().TotalMinutes < 15)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Start time and end time difference must be at least 15 minutes",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }


            //Check if the new DayOfWeek is valid
            if (requestDTO.DayOfWeek != null && !Enum.IsDefined(typeof(DayOfWeek), requestDTO.DayOfWeek))
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Invalid day of week",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }
            //Check if a PersonEmail associated with the email from the parameter PersonEmailRequestDTO exists in the database

            var personEmail = await _personRepository.GetPersonEmailByEmail(requestDTO.Email);

            if (personEmail == null)
            {
                return Unauthorized(
                    new
                    {
                        success = "false",
                        message = "Email does not exist",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }



            //Check if a PersonAvailability associated with the id from the parameter PersonAvailabilityUpdateRequestDTO exists in the database

            var personAvailability = await _personAvailabilityRepository.GetPersonAvailabilityById(requestDTO.PersonAvailabilityId);

            if (personAvailability == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Time availability not found",
                    data = new { },
                    timestamp = DateTime.Now,
                });

            }


            //Check if the account is allowed to edit this information, by checking if the PersonId from PersonEmail matches the PersonId from PersonAvailability

            if (personEmail.PersonId != personAvailability.PersonId)
            {
                return StatusCode(403, new
                {
                    success = "false",
                    message = "You do not have permission to update this time availability",
                    data = new { },
                    timestamp = DateTime.Now,
                });
            }
            //Add flag isUpdated for checking if the provided values are different from the current values
            bool isUpdated = false;

            //Check StartTime
            if (!string.IsNullOrEmpty(requestDTO.StartTime) && newStartTime.CompareTo(personAvailability.StartTime) != 0)
            {
                isUpdated = true;
                personAvailability.StartTime = newStartTime;
            }

            //Check EndTime
            if (!string.IsNullOrEmpty(requestDTO.EndTime) && newEndTime.CompareTo(personAvailability.EndTime) != 0)
            {
                isUpdated = true;
                personAvailability.EndTime = newEndTime;
            }

            //Check DayOfWeek
            if (requestDTO.DayOfWeek.HasValue && requestDTO.DayOfWeek.Value.CompareTo((int)personAvailability.DayOfWeek) != 0)
            {
                isUpdated = true;
                personAvailability.DayOfWeek = (DayOfWeek)requestDTO.DayOfWeek.Value;
            }

            //If no fields are updated, return a bad request
            if (!isUpdated)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "No new values were provided for the  update",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }

            //Convert from PersonAvailability to PersonAvailabilityDTO

            var personAvailabilityDTO = new PersonAvailabilityDTO
            {
                PersonAvailabilityId = personAvailability.PersonAvailabilityId,
                PersonId = personAvailability.PersonId,
                StartTime = personAvailability.StartTime,
                EndTime = personAvailability.EndTime,
                DayOfWeek = personAvailability.DayOfWeek,
            };

            //Attempt to update the PersonAvailability in the database

            var updateResult = await _personAvailabilityRepository.UpdatePersonAvailabilityById(personAvailabilityDTO);

            if (updateResult == null)
            {
                return StatusCode(500, new
                {
                    success = "false",
                    message = "We failed to update the time availability, please try again later",
                    data = new { },
                    timestamp = DateTime.Now,
                });
            }
            return Ok(
                new
                {
                    success = "true",
                    message = "Time availability updated successfully",
                    data = new
                    {
                        timeAvailability = new
                        PersonAvailabilitySaveResponseDTO
                        {
                            PersonAvailabilityId = updateResult.PersonAvailabilityId,
                            DayOfWeek = updateResult.DayOfWeek,
                            StartTime = updateResult.StartTime,
                            EndTime = updateResult.EndTime,
                        },
                        isUpdated
                    },
                    timestamp = DateTime.Now,
                }
            );

        }
    }
}
