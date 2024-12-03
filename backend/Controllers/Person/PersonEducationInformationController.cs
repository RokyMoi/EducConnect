using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person;
using backend.Entities.Person;
using backend.Interfaces.Person;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Person
{
    [ApiController]
    [Route("person/education")]
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
    }
}