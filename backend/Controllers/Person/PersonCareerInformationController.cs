using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person;
using backend.Interfaces.Person;
using backend.Interfaces.Reference;
using EduConnect.Entities.Person;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Person
{
    [ApiController]
    [Route("person/career")]
    public class PersonCareerInformationController(IPersonRepository _personRepository, IPersonCareerInformationRepository _personCareerInformationRepository, IReferenceRepository _referenceRepository) : ControllerBase
    {


        [HttpPost("add")]
        public async Task<IActionResult> AddCareerInformation(PersonCareerInformationControllerSaveRequestDTO saveRequestDTO)
        {

            //Check if the PersonEmail exists
            PersonEmailDTO personEmail = await _personRepository.GetPersonEmailByEmail(saveRequestDTO.Email);

            if (personEmail == null)
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "Email does not exist",
                    data = new { },
                    timestamp = DateTime.Now,
                });
            }

            //Check if the EmploymentType exists    
            var employmentType = await _referenceRepository.GetEmploymentTypeByIdAsync(saveRequestDTO.EmploymentType);

            if (employmentType == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Employment type does not exist, please choose another one",
                    data = new { },
                    timestamp = DateTime.Now,
                });
            }

            //Check if the work type exists if the property WorkType in the request is not null
            if (saveRequestDTO.WorkType != null)
            {
                var workType = await _referenceRepository.GetWorkTypeByIdAsync((int)saveRequestDTO.WorkType);
                if (workType == null)
                {
                    return NotFound(new
                    {
                        success = "false",
                        message = "Work type does not exist, please choose another one",
                        data = new { },
                        timestamp = DateTime.Now,
                    });
                }
            }

            //Assign data from PersonCareerInformationSaveRequestDTO to PersonCareerInformationCreateDTO
            PersonCareerInformationCreateDTO createDTO = new PersonCareerInformationCreateDTO
            {
                PersonId = personEmail.PersonId,
                CompanyName = saveRequestDTO.CompanyName,
                CompanyWebsite = saveRequestDTO.CompanyWebsite,
                JobTitle = saveRequestDTO.JobTitle,
                Position = saveRequestDTO.Position,
                CityOfEmployment = saveRequestDTO.CityOfEmployment,
                CountryOfEmployment = saveRequestDTO.CountryOfEmployment,
                EmploymentTypeId = saveRequestDTO.EmploymentType,
                StartDate = saveRequestDTO.StartDate,
                EndDate = saveRequestDTO.EndDate,
                JobDescription = saveRequestDTO.JobDescription,
                Responsibilities = saveRequestDTO.Responsibilities,
                Achievements = saveRequestDTO.Achievements,
                Industry = saveRequestDTO.Industry,
                SkillsUsed = saveRequestDTO.SkillsUsed,
                WorkTypeId = saveRequestDTO.WorkType,
                AdditionalInformation = saveRequestDTO.AdditionalInformation
            };

            //Attempt to save the career information for the given person
            var saveResult = await _personCareerInformationRepository.AddPersonCareerInformation(createDTO);

            if (saveResult == null)
            {
                return StatusCode(500, new
                {
                    success = "false",
                    message = "We failed to save the career information, please again later",
                    data = new { },
                    timestamp = DateTime.Now,
                });
            }
            return Ok(new
            {
                success = "true",
                message = "Career information added successfully",
                data = new
                {
                    careerInformation = new PersonCareerInformationSaveResponseDTO
                    {
                        PersonCareerInformationId = saveResult.PersonCareerInformationId,
                        CompanyName = saveResult.CompanyName,
                        CompanyWebsite = saveResult.CompanyWebsite,
                        JobTitle = saveResult.JobTitle,
                        Position = saveResult.Position,
                        CityOfEmployment = saveResult.CityOfEmployment,
                        CountryOfEmployment = saveResult.CountryOfEmployment,
                        EmploymentType = saveResult.EmploymentTypeId,
                        StartDate = saveResult.StartDate,
                        EndDate = saveResult.EndDate,
                        JobDescription = saveResult.JobDescription,
                        Responsibilities = saveResult.Responsibilities,
                        Achievements = saveResult.Achievements,
                        Industry = saveResult.Industry,
                        SkillsUsed = saveResult.SkillsUsed,
                        WorkType = saveResult.WorkTypeId,
                        AdditionalInformation = saveResult.AdditionalInformation

                    }
                },
                timestamp = DateTime.Now

            });
        }
    }
}