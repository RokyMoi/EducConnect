using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person;
using backend.DTOs.Person.PersonAvailability;
using backend.Interfaces.Person;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Person
{
    [ApiController]
    [Route("person/availability")]
    public class PersonAvailabilityController(IPersonRepository _personRepository, IPersonAvailabilityRepository _personAvailabilityRepository) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<IActionResult> AddPersonAvailability(PersonAvailabilitySaveRequestDTO saveRequestDTO)
        {

            //Verify the PersonEmail associated with the email from request exists

            var personEmail = await _personRepository.GetPersonEmailByEmail(saveRequestDTO.Email);

            if (personEmail == null)
            {
                return Unauthorized(new
                {
                    success = "false",
                    message = "Email does not exist",
                    data = new { },
                    timestamp = DateTime.Now,
                });
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
                PersonId = personEmail.PersonId,
                DayOfWeek = (DayOfWeek)saveRequestDTO.DayOfWeek,
                StartTime = startTime,
                EndTime = endTime,

            };




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
    }
}
