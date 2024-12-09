using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person;
using backend.Entities.Person;
using backend.Interfaces.Person;
using backend.Middleware;
using backend.Middleware.Tutor;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Person
{
    [ApiController]
    [Route("person/education")]
    [CheckPersonLoginSignup]
    [CheckTutorRegistration]
    public class PersonEducationInformationController(IPersonRepository _personRepository, IPersonEducationInformationRepository _personEducationInformationRepository) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<IActionResult> AddEducationInformation(PersonEducationInformationSaveRequestDTO saveRequestDTO)
        {

            //Check does given email exist
            var personEmail = await _personRepository.GetPersonEmailByEmail(saveRequestDTO.Email);
            if (personEmail == null)
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "Email does not exist",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Check if the person has already added education information
            List<PersonEducationInformationDTO> existingPersonEducationInformationList = await _personEducationInformationRepository.GetAllPersonEducationInformationByPersonId(personEmail.PersonId);


            if (existingPersonEducationInformationList != null && existingPersonEducationInformationList.Count() > 4)
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "Cannot add more than 5 education information per account",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Add education information 
            //Create new PersonEducationInformation object
            PersonEducationInformation newPersonEducationInformation = new PersonEducationInformation
            {
                PersonEducationInformationId = Guid.NewGuid(),
                PersonId = personEmail.PersonId,
                Person = personEmail.Person,
                InstitutionName = saveRequestDTO.InstitutionName,
                InstitutionOfficialWebsite = saveRequestDTO.InstitutionOfficialWebsite,
                InstitutionAddress = saveRequestDTO.InstitutionAddress,
                EducationLevel = saveRequestDTO.EducationLevel,
                FieldOfStudy = saveRequestDTO.FieldOfStudy,
                MinorFieldOfStudy = saveRequestDTO.MinorFieldOfStudy,
                StartDate = saveRequestDTO.StartDate,
                EndDate = saveRequestDTO.EndDate,
                IsCompleted = saveRequestDTO.IsCompleted,
                FinalGrade = saveRequestDTO.FinalGrade,
                Description = saveRequestDTO.Description,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ModifiedAt = null


            };

            //Attempt to add new PersonEducationInformation object
            var saveResult = _personEducationInformationRepository.AddPersonEducationInformation(newPersonEducationInformation);
            if (saveResult == null)
            {
                return StatusCode(500, new
                {
                    success = "false",
                    message = "We failed to save your education information, please try again later",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }


            return Ok(new
            {
                success = "true",
                message = "Education information added successfully",
                data = new
                {
                    EducationInformation = new PersonEducationInformationResponseDTO
                    {

                        InstitutionName = newPersonEducationInformation.InstitutionName,
                        InstitutionOfficialWebsite = newPersonEducationInformation.InstitutionOfficialWebsite,
                        InstitutionAddress = newPersonEducationInformation.InstitutionAddress,
                        EducationLevel = newPersonEducationInformation.EducationLevel,
                        FieldOfStudy = newPersonEducationInformation.FieldOfStudy,
                        MinorFieldOfStudy = newPersonEducationInformation.MinorFieldOfStudy,
                        StartDate = newPersonEducationInformation.StartDate,
                        EndDate = newPersonEducationInformation.EndDate,
                        IsCompleted = newPersonEducationInformation.IsCompleted,
                        FinalGrade = newPersonEducationInformation.FinalGrade,
                        Description = newPersonEducationInformation.Description,

                    }
                },
                timestamp = DateTime.Now
            });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdatePersonEducationInformation(PersonEducationInformationUpdateRequestDTO updateRequestDTO)
        {

            //Check if the PersonEmail with the email exists
            var personEmail = await
            _personRepository.
            GetPersonEmailByEmail(updateRequestDTO.Email);

            if (personEmail == null)
            {
                return Unauthorized(new
                {
                    success = "false",
                    message = "Email does not exist",
                    data = new { },
                    timestamp = DateTime.Now
                });

            }

            //Assign values from the updateRequestDTO to the PersonEducationInformationUpdateDTO object
            PersonEducationInformationUpdateDTO updateDTO = new PersonEducationInformationUpdateDTO
            {
                PersonEducationInformationId = updateRequestDTO.PersonEducationInformationId,
                InstitutionName = updateRequestDTO.InstitutionName,
                InstitutionOfficialWebsite = updateRequestDTO.InstitutionOfficialWebsite,
                InstitutionAddress = updateRequestDTO.InstitutionAddress,
                EducationLevel = updateRequestDTO.EducationLevel,
                FieldOfStudy = updateRequestDTO.FieldOfStudy,
                MinorFieldOfStudy = updateRequestDTO.MinorFieldOfStudy,
                StartDate = updateRequestDTO.StartDate,
                EndDate = updateRequestDTO.EndDate,
                IsCompleted = updateRequestDTO.IsCompleted,
                FinalGrade = updateRequestDTO.FinalGrade,
                Description = updateRequestDTO.Description,
            };

            //Get the PersonEducationInformation object from the database with the given PersonEducationInformationId
            var personEducationInformation = await _personEducationInformationRepository.GetPersonEducationInformationById(updateDTO.PersonEducationInformationId);
            if (personEducationInformation == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Education information not found",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }
            //Check if the PersonId from PersonEmail matches the PersonId from the PersonEducationInformation
            if (personEmail.PersonId != personEducationInformation.PersonId)
            {
                return StatusCode(403, new
                {
                    success = "false",
                    message = "You are not authorized to update this education information",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Create response object, to return with only the updated values in case of successful values update
            PersonEducationInformationUpdateResponseDTO updateResponseDTO = new PersonEducationInformationUpdateResponseDTO() { };

            //Set a flag variable to check if any value has been updated
            bool isUpdated = false;


            //Check InstitutionName
            if (updateDTO.InstitutionName != personEducationInformation.InstitutionName)
            {
                isUpdated = true;
                updateResponseDTO.InstitutionName = updateDTO.InstitutionName;

            }

            //Check InstitutionOfficialWebsite
            if (updateDTO.InstitutionOfficialWebsite != personEducationInformation.InstitutionOfficialWebsite)
            {
                isUpdated = true;
                updateResponseDTO.InstitutionOfficialWebsite = updateDTO.InstitutionOfficialWebsite;
            }

            //Check InstitutionAddress
            if (updateDTO.InstitutionAddress != personEducationInformation.InstitutionAddress)
            {
                isUpdated = true;
                updateResponseDTO.InstitutionAddress = updateDTO.InstitutionAddress;
            }

            //Check EducationLevel
            if (updateDTO.EducationLevel != null && updateDTO.EducationLevel != personEducationInformation.EducationLevel)
            {
                isUpdated = true;
                updateResponseDTO.EducationLevel = updateDTO.EducationLevel;
            }

            //Check FieldOfStudy
            if (updateDTO.FieldOfStudy != null && updateDTO.FieldOfStudy != personEducationInformation.FieldOfStudy)
            {
                isUpdated = true;
                updateResponseDTO.FieldOfStudy = updateDTO.FieldOfStudy;
            }

            //Check MinorFieldOfStudy
            if (updateDTO.MinorFieldOfStudy != personEducationInformation.MinorFieldOfStudy)
            {
                isUpdated = true;
                updateResponseDTO.MinorFieldOfStudy = updateDTO.MinorFieldOfStudy;
            }

            //Check StartDate
            if (updateDTO.StartDate != personEducationInformation.StartDate)
            {
                isUpdated = true;
                updateResponseDTO.StartDate = updateDTO.StartDate;
            }
            //Check EndDate
            if (updateDTO.EndDate != personEducationInformation.EndDate)
            {
                isUpdated = true;
                updateResponseDTO.EndDate = updateDTO.EndDate;
            }
            //Check IsCompleted
            if (updateDTO.IsCompleted.HasValue && updateDTO.IsCompleted != personEducationInformation.IsCompleted)
            {
                isUpdated = true;
                updateResponseDTO.IsCompleted = updateDTO.IsCompleted;
            }

            //Check FinalGrade 
            if (updateDTO.FinalGrade != null && updateDTO.FinalGrade != personEducationInformation.FinalGrade)
            {

                isUpdated = true;
                updateResponseDTO.FinalGrade = updateDTO.FinalGrade;
            }

            //Check Description
            if (updateDTO.Description != null && updateDTO.Description != personEducationInformation.Description)
            {
                isUpdated = true;
                updateResponseDTO.Description = updateDTO.Description;
            }

            //Check if the isUpdated flag is false, if so, return null
            if (!isUpdated)
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "No new values were provided for update",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            updateResponseDTO.PersonEducationInformationId = personEducationInformation.PersonEducationInformationId;
            //Attempt to update PersonEducationInformation
            var updateResult = await _personEducationInformationRepository.UpdatePersonEducationInformation(updateDTO);

            if (updateResult == null)
            {
                return StatusCode(500, new
                {
                    success = "false",
                    message = "Failed to update education information",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            return Ok(new
            {
                success = "true",
                message = "Education information updated successfully",
                data = new
                {
                    PersonEducationInformation = updateResponseDTO,
                },
                timestamp = DateTime.Now
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePersonEducationInformation(PersonEducationInformationDeleteRequestDTO deleteDTO)
        {
            if (deleteDTO.PersonEducationInformationId == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Invalid Id",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the PersonEmail associated with the email from the delete request body exists
            var personEmail = await _personRepository.GetPersonEmailByEmail(deleteDTO.Email);

            if (personEmail == null)
            {
                return Unauthorized(
                    new
                    {
                        success = "false",
                        message = "Email does not exist",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }



            //Check if PersonEducationInformation object with given Id exists
            var personEducationInformation = await _personEducationInformationRepository.GetPersonEducationInformationById(deleteDTO.PersonEducationInformationId);

            //If PersonEducationInformation object with given Id does not exist, return NotFound
            if (personEducationInformation == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Education information not found",
                    data = new { },
                    timestamp = DateTime.Now
                });

            }
            //Check if the PersonEmail associated with the email from the delete request body is the same as the PersonEmail associated with the PersonEducationInformation object

            if (personEmail.PersonId != personEducationInformation.PersonId)
            {
                return StatusCode(403, new
                {
                    success = "false",
                    message = "You are not authorized to delete this education information",
                    data = new { },
                    timestamp = DateTime.Now
                });

            }
            //Attempt to delete PersonEducationInformation object with the given Id
            var deleteResult = await _personEducationInformationRepository.DeletePersonEducationInformationById(deleteDTO.PersonEducationInformationId);

            //If deleteResult is null, return InternalServerError
            if (deleteResult == null)
            {
                return StatusCode(500, new
                {
                    success = "false",
                    message = "Failed to delete education information, please try again later",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            return Ok(new
            {
                success = "true",
                message = "Education information deleted successfully",
                data = new { },
                timestamp = DateTime.Now
            });
        }

        //POST All method, (GET not used for security purposes, GET sends data to server using URL, while POST uses body)returns all PersonEducationInformation objects related to the given Person object
        [HttpPost("all")]
        public async Task<IActionResult> GetAllPersonEducationInformation(PersonEmailRequestDTO personEmailRequestDTO)
        {

            //Get PersonId by Email
            var personEmail = await _personRepository.GetPersonEmailByEmail(personEmailRequestDTO.Email);

            if (personEmail == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Email does not exist",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Get PersonEducationInformation object with the given PersonId
            var personEducationInformationList = await _personEducationInformationRepository.GetAllPersonEducationInformationByPersonId(personEmail.PersonId);

            if (personEducationInformationList == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "No education information found for account",
                    data = new { },
                    timestamp = DateTime.Now
                });

            }

            //Convert found List of PersonEducationInformationDTOs to List of PersonEducationInformationGetResponseDTOs
            List<PersonEducationInformationGetResponseDTO> responseDTOList = new List<PersonEducationInformationGetResponseDTO>();
            foreach (var personEducationInformation in personEducationInformationList)
            {
                PersonEducationInformationGetResponseDTO responseDTO = new PersonEducationInformationGetResponseDTO
                {
                    PersonEducationInformationId = personEducationInformation.PersonEducationInformationId,
                    InstitutionName = personEducationInformation.InstitutionName,
                    InstitutionOfficialWebsite = personEducationInformation.InstitutionOfficialWebsite,
                    InstitutionAddress = personEducationInformation.InstitutionAddress,
                    EducationLevel = personEducationInformation.EducationLevel,
                    FieldOfStudy = personEducationInformation.FieldOfStudy,
                    MinorFieldOfStudy = personEducationInformation.MinorFieldOfStudy,
                    StartDate = personEducationInformation.StartDate,
                    EndDate = personEducationInformation.EndDate,
                    IsCompleted = personEducationInformation.IsCompleted,
                    FinalGrade = personEducationInformation.FinalGrade,
                    Description = personEducationInformation.Description,
                };
                responseDTOList.Add(responseDTO);
            }


            return Ok(new
            {
                success = "true",
                message = "Successfully retrieved " + personEducationInformationList.Count + " entires for education information for the given account",
                data = new
                {
                    PersonEducationInformation = responseDTOList,
                }

            });
        }

        //POST method, (GET not used for security purposes, GET sends data to server using URL, while POST uses body)returns PersonEducationInformation with given Id
        [HttpPost("get")]
        public async Task<IActionResult> GetPersonEducationInformationById(PersonEducationInformationGetRequestDTO requestDTO)
        {

            Console.WriteLine("RequestDTO: " + requestDTO.PersonEducationInformationId);
            if (requestDTO.PersonEducationInformationId == Guid.Empty)
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "PersonEducationInformationId is required",
                    data = new { },
                    timestamp = DateTime.Now
                });

            }

            //Check does the PersonEmail with the given Email exist
            var personEmail = await _personRepository.GetPersonEmailByEmail(requestDTO.Email);
            if (personEmail == null)
            {
                return Unauthorized(new
                {
                    success = "false",
                    message = "Email does not exist",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }


            //Get PersonEducationInformation object with the given PersonId
            var personEducationInformation = await _personEducationInformationRepository.GetPersonEducationInformationById(requestDTO.PersonEducationInformationId);

            if (personEducationInformation == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "No education information found for account",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Convert PersonEducationInformation to PersonEducationInformationGetResponseDTO

            return Ok(
                new
                {
                    success = "true",
                    message = "Successfully retrieved education information for the given account",
                    data = new
                    {
                        PersonEducationInformation = new PersonEducationInformationGetResponseDTO
                        {
                            PersonEducationInformationId = personEducationInformation.PersonEducationInformationId,
                            InstitutionName = personEducationInformation.InstitutionName,
                            InstitutionOfficialWebsite = personEducationInformation.InstitutionOfficialWebsite,
                            InstitutionAddress = personEducationInformation.InstitutionAddress,
                            EducationLevel = personEducationInformation.EducationLevel,
                            FieldOfStudy = personEducationInformation.FieldOfStudy,
                            MinorFieldOfStudy = personEducationInformation.MinorFieldOfStudy,
                            StartDate = personEducationInformation.StartDate,
                            EndDate = personEducationInformation.EndDate,
                            IsCompleted = personEducationInformation.IsCompleted,
                            FinalGrade = personEducationInformation.FinalGrade,
                            Description = personEducationInformation.Description,
                        }
                    }
                }
            );
        }

    }


}
