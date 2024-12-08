using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.DataSeeder;
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

        [HttpPost("all")]
        public async Task<IActionResult> GetPersonCareerInfromation(PersonEmailRequestDTO requestDTO)
        {

            //Check if the PersonEmail with the provided email value exists
            PersonEmailDTO personEmail = await _personRepository.GetPersonEmailByEmail(requestDTO.Email);

            if (personEmail == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Email does not exist",
                    data = new { },
                    timestamp = DateTime.Now,
                });
            }

            //Get all career information with the PersonId taken from the PersonEmail
            var careerInformationList = await _personCareerInformationRepository.GetAllPersonCareerInformationByPersonId(personEmail.PersonId);

            if (careerInformationList == null || careerInformationList.Count == 0)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "No career information found for this account",
                    data = new { },
                    timestamp = DateTime.Now,
                });

            }

            //Convert the list of career information to a list of PersonCareerInformationSaveResponseDTOs

            List<PersonCareerInformationSaveResponseDTO> careerInformationListResponse = new List<PersonCareerInformationSaveResponseDTO>();
            foreach (var careerInformation in careerInformationList)
            {
                careerInformationListResponse.Add(new PersonCareerInformationSaveResponseDTO
                {
                    PersonCareerInformationId = careerInformation.PersonCareerInformationId,
                    CompanyName = careerInformation.CompanyName,
                    CompanyWebsite = careerInformation.CompanyWebsite,
                    JobTitle = careerInformation.JobTitle,
                    Position = careerInformation.Position,
                    CityOfEmployment = careerInformation.CityOfEmployment,
                    CountryOfEmployment = careerInformation.CountryOfEmployment,
                    EmploymentType = careerInformation.EmploymentTypeId,
                    StartDate = careerInformation.StartDate,
                    EndDate = careerInformation.EndDate,
                    JobDescription = careerInformation.JobDescription,
                    Responsibilities = careerInformation.Responsibilities,
                    Achievements = careerInformation.Achievements,
                    Industry = careerInformation.Industry,
                    SkillsUsed = careerInformation.SkillsUsed,
                    WorkType = careerInformation.WorkTypeId,
                    AdditionalInformation = careerInformation.AdditionalInformation
                });
            }

            return Ok(
                new
                {
                    success = "true",
                    message = $"{careerInformationListResponse.Count} records of career information for this account have been found",
                    data = new
                    {
                        CareerInformation = careerInformationListResponse,
                    },
                    timestamp = DateTime.Now

                }
            );



        }

        [HttpGet]
        public async Task<IActionResult> GetPersonCareerInformationById([FromQuery] Guid id)
        {

            if (id == Guid.Empty)
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "Invalid id",
                    data = new { },
                    timestamp = DateTime.Now,
                });
            }

            //Get the career information with the provided id
            var careerInformation = await _personCareerInformationRepository.GetPersonCareerInformationById(id);

            if (careerInformation == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Career information not found",
                    data = new { },
                    timestamp = DateTime.Now,
                });
            };

            return Ok(
                new
                {
                    success = "true",
                    message = "Career information found",
                    data = new
                    {
                        careerInformation = new PersonCareerInformationSaveResponseDTO
                        {
                            PersonCareerInformationId = careerInformation.PersonCareerInformationId,
                            CompanyName = careerInformation.CompanyName,
                            CompanyWebsite = careerInformation.CompanyWebsite,
                            JobTitle = careerInformation.JobTitle,
                            Position = careerInformation.Position,
                            CityOfEmployment = careerInformation.CityOfEmployment,
                            CountryOfEmployment = careerInformation.CountryOfEmployment,
                            EmploymentType = careerInformation.EmploymentTypeId,
                            StartDate = careerInformation.StartDate,
                            EndDate = careerInformation.EndDate,
                            JobDescription = careerInformation.JobDescription,
                            Responsibilities = careerInformation.Responsibilities,
                            Achievements = careerInformation.Achievements,
                            Industry = careerInformation.Industry,
                            SkillsUsed = careerInformation.SkillsUsed,
                            WorkType = careerInformation.WorkTypeId,
                            AdditionalInformation = careerInformation.AdditionalInformation
                        }
                    },
                    timestamp = DateTime.Now
                }
            );
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdatePersonCareerInformation(PersonCareerInformationUpdateRequestDTO updateRequestDTO)
        {

            //Check if the provided email value is connected to an existing PersonEmail object 
            var personEmail = await _personRepository.GetPersonEmailByEmail(updateRequestDTO.Email);

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


            //Get the PersonCareerInformation object with the provided id
            var careerInformation = await _personCareerInformationRepository.GetPersonCareerInformationById(updateRequestDTO.PersonCareerInformationId);

            if (careerInformation == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Career information not found",
                    data = new { },
                    timestamp = DateTime.Now,
                });
            }

            //Check if the career information is associated with the account

            if (careerInformation.PersonId != personEmail.PersonId)
            {
                return Unauthorized(new
                {
                    success = "false",
                    message = "Career information does not belong to the account",
                    data = new { },
                    timestamp = DateTime.Now,
                });
            }


            //Update the career information

            //Set flag isUpdated to keep track of whether any fields were updated
            bool isUpdated = false;

            //Check CompanyName 
            if (updateRequestDTO.CompanyName != null && updateRequestDTO.CompanyName != careerInformation.CompanyName)
            {

                careerInformation.CompanyName = updateRequestDTO.CompanyName;
                isUpdated = true;
            }

            //Check CompanyWebsite

            if (updateRequestDTO.UpdateCompanyWebsite && updateRequestDTO.CompanyWebsite != careerInformation.CompanyWebsite)
            {
                careerInformation.CompanyWebsite = updateRequestDTO.CompanyWebsite;
                isUpdated = true;
            }

            //Check JobTitle 
            if (updateRequestDTO.JobTitle != null && updateRequestDTO.JobTitle != careerInformation.JobTitle)
            {
                careerInformation.JobTitle = updateRequestDTO.JobTitle;
                isUpdated = true;
            }

            //Check Position
            if (updateRequestDTO.UpdatePosition && updateRequestDTO.Position != careerInformation.Position)
            {
                careerInformation.Position = updateRequestDTO.Position;
                isUpdated = true;
            }

            //Check CityOfEmployment
            if (updateRequestDTO.CityOfEmployment != null && !updateRequestDTO.CityOfEmployment.Equals(careerInformation.CityOfEmployment))
            {
                careerInformation.CityOfEmployment = updateRequestDTO.CityOfEmployment;
                isUpdated = true;
            }

            //Check CountryOfEmployment
            if (updateRequestDTO.CountryOfEmployment != null && !updateRequestDTO.CountryOfEmployment.Equals(careerInformation.CountryOfEmployment))
            {
                careerInformation.CountryOfEmployment = updateRequestDTO.CountryOfEmployment;
                isUpdated = true;
            }

            //Check EmploymentType
            if (updateRequestDTO.EmploymentType != null && !updateRequestDTO.EmploymentType.Value.Equals(careerInformation.EmploymentTypeId))
            {
                //Check if the provided EmploymentType exists in the database
                var employmentType = await _referenceRepository.GetEmploymentTypeByIdAsync(updateRequestDTO.EmploymentType.Value);
                if (employmentType == null)
                {
                    return NotFound(
                        new
                        {
                            success = "false",
                            message = "Employment type not found, please select another one",
                            data = new { },
                            timestamp = DateTime.Now,
                        }
                    );
                }
                careerInformation.EmploymentTypeId = updateRequestDTO.EmploymentType.Value;
                isUpdated = true;
            }



            //Check StartDate 
            if (updateRequestDTO.StartDate != null && !updateRequestDTO.StartDate.Value.ToString().Equals(careerInformation.StartDate.ToString()))
            {

                careerInformation.StartDate = updateRequestDTO.StartDate.Value;
                isUpdated = true;
            }


            //Check EndDate 
            if (updateRequestDTO.UpdateEndDate && !updateRequestDTO.EndDate.Value.ToString().Equals(careerInformation.EndDate.Value.ToString()))
            {
                careerInformation.EndDate = updateRequestDTO.EndDate;
                isUpdated = true;
            }

            //Check JobDescription
            if (updateRequestDTO.UpdateJobDescription && updateRequestDTO.JobDescription != careerInformation.JobDescription)
            {
                careerInformation.JobDescription = updateRequestDTO.JobDescription;
                isUpdated = true;
            }

            //Check Responsibilities
            if (updateRequestDTO.UpdateResponsibilities && updateRequestDTO.Responsibilities != careerInformation.Responsibilities)
            {
                careerInformation.Responsibilities = updateRequestDTO.Responsibilities;
                isUpdated = true;
            }

            //Check Achievements
            if (updateRequestDTO.UpdateAchievements && updateRequestDTO.Achievements != careerInformation.Achievements)
            {
                careerInformation.Achievements = updateRequestDTO.Achievements;
                isUpdated = true;
            }

            //Check Industry 
            if (updateRequestDTO.Industry != null && updateRequestDTO.Industry != careerInformation.Industry)
            {
                careerInformation.Industry = updateRequestDTO.Industry;
                isUpdated = true;
            }

            //Check SkillsUsed
            if (updateRequestDTO.SkillsUsed != null && updateRequestDTO.SkillsUsed != careerInformation.SkillsUsed)
            {
                careerInformation.SkillsUsed = updateRequestDTO.SkillsUsed;
                isUpdated = true;

            }

            //Check WorkType
            if (updateRequestDTO.UpdateWorkType && updateRequestDTO.WorkType != careerInformation.WorkTypeId)
            {

                //Check if the provided WorkType exists in the database if it is not null in the request
                if (updateRequestDTO.WorkType.HasValue)
                {

                    var workType = await _referenceRepository.GetWorkTypeByIdAsync(updateRequestDTO.WorkType.Value);
                    if (workType == null)
                    {
                        return NotFound(
                            new
                            {
                                success = "false",
                                message = "Work type not found, please select another one",
                                data = new { },
                                timestamp = DateTime.Now,
                            }
                        );
                    }
                }
                careerInformation.WorkTypeId = updateRequestDTO.WorkType;
                isUpdated = true;
            }

            //Check AdditionalInformation
            if (updateRequestDTO.UpdateAdditionalInformation && updateRequestDTO.AdditionalInformation != careerInformation.AdditionalInformation)
            {
                careerInformation.AdditionalInformation = updateRequestDTO.AdditionalInformation;
                isUpdated = true;
            }

            //If no fields were updated, return a 400 Bad Request response

            if (!isUpdated)
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "No fields were provided for update",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Convert from PersonCareerInformation object to PersonCareerInformationDTO
            PersonCareerInformationDTO careerInformationDTO = new PersonCareerInformationDTO()
            {
                PersonCareerInformationId = careerInformation.PersonCareerInformationId,
                PersonId = careerInformation.PersonId,
                CompanyName = careerInformation.CompanyName,
                CompanyWebsite = careerInformation.CompanyWebsite,
                JobTitle = careerInformation.JobTitle,
                Position = careerInformation.Position,
                CityOfEmployment = careerInformation.CityOfEmployment,
                CountryOfEmployment = careerInformation.CountryOfEmployment,
                EmploymentTypeId = careerInformation.EmploymentTypeId,
                StartDate = careerInformation.StartDate,
                EndDate = careerInformation.EndDate,
                JobDescription = careerInformation.JobDescription,
                Responsibilities = careerInformation.Responsibilities,
                Achievements = careerInformation.Achievements,
                Industry = careerInformation.Industry,
                SkillsUsed = careerInformation.SkillsUsed,
                WorkTypeId = careerInformation.WorkTypeId,
                AdditionalInformation = careerInformation.AdditionalInformation,
            };

            //Attempt to update the career information in the database 
            var updateResult = await _personCareerInformationRepository.UpdatePersonCareerInformation(careerInformationDTO);

            if (updateResult == null)
            {
                return StatusCode(500, new
                {
                    success = "false",
                    message = "We failed to update your career information, please try again later",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }
            return Ok(new
            {
                success = "true",
                message = "Career information updated successfully",
                data = new
                {
                    careerInformation = new PersonCareerInformationSaveResponseDTO
                    {
                        PersonCareerInformationId = updateResult.PersonCareerInformationId,
                        CompanyName = updateResult.CompanyName,
                        CompanyWebsite = updateResult.CompanyWebsite,
                        JobTitle = updateResult.JobTitle,
                        Position = updateResult.Position,
                        CityOfEmployment = updateResult.CityOfEmployment,
                        CountryOfEmployment = updateResult.CountryOfEmployment,
                        EmploymentType = updateResult.EmploymentTypeId,
                        StartDate = updateResult.StartDate,
                        EndDate = updateResult.EndDate,
                        JobDescription = updateResult.JobDescription,
                        Responsibilities = updateResult.Responsibilities,
                        Achievements = updateResult.Achievements,
                        Industry = updateResult.Industry,
                        SkillsUsed = updateResult.SkillsUsed,
                        WorkType = updateResult.WorkTypeId,
                        AdditionalInformation = updateResult.AdditionalInformation,
                    }
                },
                timestamp = DateTime.Now
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePersonCareerInformation(PersonCareerInformationDeleteRequest deleteRequestDTO)
        {

            //Check if the PersonEducationInformationId is valid
            if (deleteRequestDTO.PersonCareerInformationId == Guid.Empty)
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "Invalid Id",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Check if the PersonEmail associated with the email from the request body exists
            var personEmail = await _personRepository.GetPersonEmailByEmail(deleteRequestDTO.Email);

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

            //Check if the PersonCareerInformationId exists in the database

            var personCareerInformation = await _personCareerInformationRepository.GetPersonCareerInformationById(deleteRequestDTO.PersonCareerInformationId);

            if (personCareerInformation == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "Career information not found",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Check if the PersonId from PersonEmail is authorized to delete the PersonCareerInformation
            if (personEmail.PersonId != personCareerInformation.PersonId)
            {
                return StatusCode(403, new
                {
                    success = "false",
                    message = "You are not authorized to delete this career information",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Delete the PersonCareerInformation from the database
            var deleteResult = await _personCareerInformationRepository.DeletePersonCareerInformationById(deleteRequestDTO.PersonCareerInformationId);

            if (deleteResult == null)
            {
                return StatusCode(500, new
                {
                    success = "false",
                    message = "We failed to delete your career information, please try again later",
                    data = new { },
                    timestamp = DateTime.Now
                });

            }

            return Ok(
                new
                {
                    success = "true",
                    message = "Career information deleted successfully",
                    data = new
                    {


                        careerInformation = new PersonCareerInformationSaveResponseDTO
                        {
                            PersonCareerInformationId = deleteResult.PersonCareerInformationId,
                            CompanyName = deleteResult.CompanyName,
                            CompanyWebsite = deleteResult.CompanyWebsite,
                            JobTitle = deleteResult.JobTitle,
                            Position = deleteResult.Position,
                            CityOfEmployment = deleteResult.CityOfEmployment,
                            CountryOfEmployment = deleteResult.CountryOfEmployment,
                            EmploymentType = deleteResult.EmploymentTypeId,
                            StartDate = deleteResult.StartDate,
                            EndDate = deleteResult.EndDate,
                            JobDescription = deleteResult.JobDescription,
                            Responsibilities = deleteResult.Responsibilities,
                            Achievements = deleteResult.Achievements,
                            Industry = deleteResult.Industry,
                            SkillsUsed = deleteResult.SkillsUsed,
                            WorkType = deleteResult.WorkTypeId,
                            AdditionalInformation = deleteResult.AdditionalInformation,

                        }

                    },
                    timestamp = DateTime.Now,
                }
            );
        }


    }
}
