using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data.DataSeeder;
using backend.DTOs.Person;
using backend.Interfaces.Person;
using backend.Interfaces.Reference;
using backend.Interfaces.Tutor;
using backend.Middleware;
using EduConnect.Entities.Person;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Person
{
    [ApiController]
    [Route("person/career")]
    [CheckPersonLoginSignup]
    public class PersonCareerInformationController(IPersonRepository _personRepository, IPersonCareerInformationRepository _personCareerInformationRepository, IReferenceRepository _referenceRepository, ITutorRepository _tutorRepository,
    backend.Interfaces.Reference.ICountryRepository _countryRepository
    ) : ControllerBase
    {


        [HttpPost]
        public async Task<IActionResult> AddCareerInformation(PersonCareerInformationControllerSaveRequestDTO saveRequestDTO)
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
            //If tutor is not null, check the TutorRegistrationStatus is below 3 (Personal Information, status before)
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



            //Check if the EmploymentType exists    
            var employmentType = await _referenceRepository.GetEmploymentTypeByIdAsync(saveRequestDTO.EmploymentTypeId);

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


            //Check if the IndustryClassification exists 
            var industryClassification = await _referenceRepository.GetIndustryClassificationByIdAsync(saveRequestDTO.IndustryClassificationId);

            if (industryClassification == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Industry classification does not exist, please choose another one",
                        data = new { },
                        timestamp = DateTime.Now,
                    }
                );
            }
            //Assign data from PersonCareerInformationSaveRequestDTO to PersonCareerInformationCreateDTO
            PersonCareerInformationCreateDTO createDTO = new PersonCareerInformationCreateDTO
            {
                PersonId = personId,
                CompanyName = saveRequestDTO.CompanyName,
                CompanyWebsite = saveRequestDTO.CompanyWebsite,
                JobTitle = saveRequestDTO.JobTitle,
                Position = saveRequestDTO.Position,
                CityOfEmployment = saveRequestDTO.CityOfEmployment,
                CountryOfEmployment = saveRequestDTO.CountryOfEmployment,
                EmploymentTypeId = saveRequestDTO.EmploymentTypeId,
                StartDate = saveRequestDTO.StartDate,
                EndDate = saveRequestDTO.EndDate,
                JobDescription = saveRequestDTO.JobDescription,
                Responsibilities = saveRequestDTO.Responsibilities,
                Achievements = saveRequestDTO.Achievements,
                IndustryClassificationId = saveRequestDTO.IndustryClassificationId,
                SkillsUsed = saveRequestDTO.SkillsUsed,
                WorkTypeId = saveRequestDTO.WorkType,
                AdditionalInformation = saveRequestDTO.AdditionalInformation
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
                        EmploymentTypeId = saveResult.EmploymentTypeId,
                        StartDate = saveResult.StartDate,
                        EndDate = saveResult.EndDate,
                        JobDescription = saveResult.JobDescription,
                        Responsibilities = saveResult.Responsibilities,
                        Achievements = saveResult.Achievements,
                        IndustryClassificationId = saveResult.IndustryClassificationId,
                        SkillsUsed = saveResult.SkillsUsed,
                        WorkTypeId = saveResult.WorkTypeId,
                        AdditionalInformation = saveResult.AdditionalInformation

                    }
                },
                timestamp = DateTime.Now

            });
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetPersonCareerInfromation()
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
            if (tutor != null && tutor.TutorRegistrationStatusId < 4)
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



            //Get all career information with the PersonId taken from the PersonEmail
            var careerInformationList = await _personCareerInformationRepository.GetAllPersonCareerInformationByPersonId(personId);

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
                    EmploymentTypeId = careerInformation.EmploymentTypeId,
                    StartDate = careerInformation.StartDate,
                    EndDate = careerInformation.EndDate,
                    JobDescription = careerInformation.JobDescription,
                    Responsibilities = careerInformation.Responsibilities,
                    Achievements = careerInformation.Achievements,
                    IndustryClassificationId = careerInformation.IndustryClassificationId,
                    Industry = careerInformation.Industry,
                    Sector = careerInformation.Sector,
                    SkillsUsed = careerInformation.SkillsUsed,
                    WorkTypeId = careerInformation.WorkTypeId,
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
                            EmploymentTypeId = careerInformation.EmploymentTypeId,
                            StartDate = careerInformation.StartDate,
                            EndDate = careerInformation.EndDate,
                            JobDescription = careerInformation.JobDescription,
                            Responsibilities = careerInformation.Responsibilities,
                            Achievements = careerInformation.Achievements,
                            IndustryClassificationId = careerInformation.IndustryClassificationId,
                            SkillsUsed = careerInformation.SkillsUsed,
                            WorkTypeId = careerInformation.WorkTypeId,
                            AdditionalInformation = careerInformation.AdditionalInformation
                        }
                    },
                    timestamp = DateTime.Now
                }
            );
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePersonCareerInformation(PersonCareerInformationUpdateRequestDTO updateRequestDTO)
        {

            //Check if the PersonCareerInformationId is Guid.Empty
            if (updateRequestDTO.PersonCareerInformationId == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Career information id is required",
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
            if (tutor != null && tutor.TutorRegistrationStatusId < 4)
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

            //Get the PersonCareerInformation with the PersonCareerInformationId from the updateRequestDTO
            var personCareerInformation = await _personCareerInformationRepository.GetPersonCareerInformationById(updateRequestDTO.PersonCareerInformationId);


            if (personCareerInformation == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Career information not found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            var currentPersonCareerInformation = personCareerInformation;

            //Check if the PersonId from the HttpContext is authorized user, to update the PersonCareerInformation
            if (personCareerInformation.PersonId != personId)
            {
                return Unauthorized(
                    new
                    {
                        success = "false",
                        message = "You are not authorized to update this career information",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Flag to track were any fields provided to update
            bool isUpdated = false;

            //Validation checks

            //Check if the companyName should be updated
            if (updateRequestDTO.updateCompanyName)
            {
                //Check if the companyName is null or empty 
                if (string.IsNullOrEmpty(updateRequestDTO.CompanyName))
                {
                    return BadRequest(
                        new
                        {
                            success = "false",
                            message = "To update the company name, you must provide a company name",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }

                //Check if the companyName is different from the one in the database

                if (personCareerInformation.CompanyName != updateRequestDTO.CompanyName)
                {
                    personCareerInformation.CompanyName = updateRequestDTO.CompanyName;
                    isUpdated = true;
                }
            }

            //Check if the companyWebsite should be updated
            if (updateRequestDTO.updateCompanyWebsite)
            {

                //Check if the companyWebsite is different from the one in the database
                if (personCareerInformation.CompanyWebsite != updateRequestDTO.CompanyWebsite)
                {
                    personCareerInformation.CompanyWebsite = updateRequestDTO.CompanyWebsite;
                    isUpdated = true;
                }
            }


            //Check if the jobTitle should be updated
            if (updateRequestDTO.updateJobTitle)
            {

                //Check if the jobTitle is null or empty 
                if (string.IsNullOrEmpty(updateRequestDTO.JobTitle))
                {
                    return BadRequest(
                        new
                        {
                            success = "false",
                            message = "To update the job title, you must provide a new job title",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }

                //Check if the jobTitle is different from the one in the database

                if (personCareerInformation.JobTitle != updateRequestDTO.JobTitle)
                {
                    personCareerInformation.JobTitle = updateRequestDTO.JobTitle;
                    isUpdated = true;
                }
            }

            //Check if the position should be updated
            if (updateRequestDTO.updatePosition)
            {

                //Check if the position is different from the one in the database
                if (personCareerInformation.Position != updateRequestDTO.Position)
                {
                    personCareerInformation.Position = updateRequestDTO.Position;
                    isUpdated = true;
                }
            }

            //Check if the cityOfEmployment should be updated
            if (updateRequestDTO.updateCityOfEmployment)
            {
                //Check if the cityOfEmployment is null or empty 
                if (string.IsNullOrEmpty(updateRequestDTO.CityOfEmployment))
                {
                    return BadRequest(
                        new
                        {
                            success = "false",
                            message = "To update the city of employment, you must provide a new city of employment",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }

                //Check if the companyName is different from the one in the database

                if (personCareerInformation.CityOfEmployment != updateRequestDTO.CityOfEmployment)
                {
                    personCareerInformation.CityOfEmployment = updateRequestDTO.CityOfEmployment;
                    isUpdated = true;
                }
            }

            //Check if the countryOfEmployment should be updated
            if (updateRequestDTO.updateCountryOfEmployment)
            {
                //Check if the countryOfEmployment is null or empty 
                if (string.IsNullOrEmpty(updateRequestDTO.CountryOfEmployment))
                {
                    return BadRequest(
                        new
                        {
                            success = "false",
                            message = "To update the country of employment, you must provide a new country of employment",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }


                //Check if the countryOfEmployment matches any of the records in the database table's Country column Name

                var country = await _countryRepository.GetCountryByName(updateRequestDTO.CountryOfEmployment);

                if (country == null)
                {
                    return BadRequest(
                        new
                        {
                            success = "false",
                            message = "The country of employment does not exist, please provide a valid country of employment",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }
                //Check if the companyName is different from the one in the database

                if (personCareerInformation.CountryOfEmployment != updateRequestDTO.CountryOfEmployment)
                {
                    personCareerInformation.CountryOfEmployment = updateRequestDTO.CountryOfEmployment;
                    isUpdated = true;
                }
            }



            //Check if the employmentTypeId should be updated
            if (updateRequestDTO.updateEmploymentTypeId)
            {
                //Check if the cityOfEmployment is null or empty 
                if (updateRequestDTO.EmploymentTypeId == null)
                {
                    return BadRequest(
                        new
                        {
                            success = "false",
                            message = "To update the employment type, you must provide a new employment type",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }

                //Check if the employmentTypeId matches any of the records in the database table's EmploymentType column EmploymentTypeId

                var employmentType = await _referenceRepository.GetEmploymentTypeByIdAsync((int)updateRequestDTO.EmploymentTypeId);
                if (employmentType == null)
                {
                    return NotFound(
                        new
                        {
                            success = "false",
                            message = "The employment type does not exist, please provide a valid employment type",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }



                //Check if the employmentTypeId is different from the one in the database

                if (personCareerInformation.EmploymentTypeId != updateRequestDTO.EmploymentTypeId)
                {
                    personCareerInformation.EmploymentTypeId = (int)updateRequestDTO.EmploymentTypeId;
                    isUpdated = true;
                }
            }


            //Check if the startDate should be updated
            if (updateRequestDTO.updateStartDate)
            {
                //Check if the startDate is null or empty 
                if (updateRequestDTO.StartDate == null)
                {
                    return BadRequest(
                        new
                        {
                            success = "false",
                            message = "To update the start date of employment, you must provide a new start date",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }

                //Check if the startDate is different from the one in the database

                if (personCareerInformation.StartDate != updateRequestDTO.StartDate)
                {
                    personCareerInformation.StartDate = (DateOnly)updateRequestDTO.StartDate;
                    isUpdated = true;
                }
            }

            //Check if the endDate should be updated
            if (updateRequestDTO.updateEndDate)
            {


                //Check if the endDate is different from the one in the database

                if (personCareerInformation.EndDate != updateRequestDTO.EndDate)
                {
                    personCareerInformation.EndDate = (DateOnly)updateRequestDTO.EndDate;
                    isUpdated = true;
                }
            }

            //Check if the jobDescription should be updated
            if (updateRequestDTO.updateJobDescription)
            {


                //Check if the jobDescription is different from the one in the database

                if (personCareerInformation.JobDescription != updateRequestDTO.JobDescription)
                {
                    personCareerInformation.JobDescription = updateRequestDTO.JobDescription;
                    isUpdated = true;
                }
            }

            //Check if the responsibilities should be updated
            if (updateRequestDTO.updateResponsibilities)
            {


                //Check if the responsibilities is different from the one in the database

                if (personCareerInformation.Responsibilities != updateRequestDTO.Responsibilities)
                {
                    personCareerInformation.Responsibilities = updateRequestDTO.Responsibilities;
                    isUpdated = true;
                }
            }

            //Check if the achievements should be updated
            if (updateRequestDTO.updateAchievements)
            {


                //Check if the achievements is different from the one in the database

                if (personCareerInformation.Achievements != updateRequestDTO.Achievements)
                {
                    personCareerInformation.Achievements = updateRequestDTO.Achievements;
                    isUpdated = true;
                }
            }

            //Check if the industryClassificationId should be updated
            if (updateRequestDTO.updateIndustryClassificationId)
            {
                //Check if the industryClassificationId is null or Guid.Empty
                if (updateRequestDTO.IndustryClassificationId == null || updateRequestDTO.IndustryClassificationId == Guid.Empty)
                {
                    return BadRequest(new
                    {
                        success = "false",
                        message = "To update the industry classification, you must provide a valid industry classification",
                        data = new { },
                        timestamp = DateTime.Now
                    });
                }

                //Check if the provided industryClassificationId matches a record from the database table IndustryClassification column IndustryClassificationId
                var industryClassification = await _referenceRepository.GetIndustryClassificationByIdAsync((Guid)updateRequestDTO.IndustryClassificationId);

                if (industryClassification == null)
                {
                    return BadRequest(
                        new
                        {
                            success = "false",
                            message = "The provided industry classification does not exist",
                            data = new { },
                            timestamp = DateTime.Now
                        }
                    );
                }

                //Check if the industryClassificationId is different from the one in the database
                if (personCareerInformation.IndustryClassificationId != updateRequestDTO.IndustryClassificationId)
                {
                    personCareerInformation.IndustryClassificationId = (Guid)updateRequestDTO.IndustryClassificationId;
                    isUpdated = true;
                }
            }

            //Check if the skillsUsed should be updated
            if (updateRequestDTO.updateSkillsUsed)
            {
                //Check if the skillsUsed is null or empty
                if (string.IsNullOrEmpty(updateRequestDTO.SkillsUsed))
                {
                    return BadRequest(new
                    {
                        success = "false",
                        message = "To update the skills used, you must provide at least one skill",
                        data = new { },
                        timestamp = DateTime.Now
                    });
                }
                //Check if the skillsUsed is different from the one in the database
                if (personCareerInformation.SkillsUsed != updateRequestDTO.SkillsUsed)
                {
                    personCareerInformation.SkillsUsed = updateRequestDTO.SkillsUsed;
                    isUpdated = true;
                }
            }

            //Check if the workTypeId should be updated
            if (updateRequestDTO.updateWorkTypeId)
            {

                //Check if the workTypeId if not null exists in the database
                if (updateRequestDTO.WorkTypeId != null)
                {
                    var workType = await _referenceRepository.GetWorkTypeByIdAsync((int)updateRequestDTO.WorkTypeId);

                    if (workType == null)
                    {
                        return BadRequest(new
                        {
                            success = "false",
                            message = "The provided work type does not exist",
                            data = new { },
                            timestamp = DateTime.Now
                        });
                    }

                    //Check if the workTypeId is different from the one in the database
                    if (personCareerInformation.WorkTypeId != updateRequestDTO.WorkTypeId)
                    {
                        personCareerInformation.WorkTypeId = (int)updateRequestDTO.WorkTypeId;
                        isUpdated = true;
                    }
                }


            }

            //Check if the additionalInformation should be updated
            if (updateRequestDTO.updateAdditionalInformation)
            {

                //Check if the additionalInformation is different from the one in the database
                if (personCareerInformation.AdditionalInformation != updateRequestDTO.AdditionalInformation)
                {
                    personCareerInformation.AdditionalInformation = updateRequestDTO.AdditionalInformation;
                    isUpdated = true;
                }
            }

            //Check if any of the fields were updated
            if (!isUpdated)
            {
                return BadRequest(new
                {
                    success = "false",
                    message = "No new values to update were provided",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Update the personCareerInformation in the database
            var updateResult = await _personCareerInformationRepository.UpdatePersonCareerInformation(
                personCareerInformation
            );

            if (updateResult == null)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "false",
                        message = "An error occurred while updating the person career information, plase try again later",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            return Ok(
                new
                {
                    success = "true",
                    message = "Person career information updated successfully",
                    data = new { },
                    timestamp = DateTime.Now
                });

        }

        [HttpDelete]
        public async Task<IActionResult> DeletePersonCareerInformation(PersonCareerInformationDeleteRequest deleteRequestDTO)
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
            //If tutor is not null, check the TutorRegistrationStatus is below 3 (Personal Information, status before)
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
            if (personId != personCareerInformation.PersonId)
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
                            EmploymentTypeId = deleteResult.EmploymentTypeId,
                            StartDate = deleteResult.StartDate,
                            EndDate = deleteResult.EndDate,
                            JobDescription = deleteResult.JobDescription,
                            Responsibilities = deleteResult.Responsibilities,
                            Achievements = deleteResult.Achievements,
                            IndustryClassificationId = deleteResult.IndustryClassificationId,
                            SkillsUsed = deleteResult.SkillsUsed,
                            WorkTypeId = deleteResult.WorkTypeId,
                            AdditionalInformation = deleteResult.AdditionalInformation,

                        }

                    },
                    timestamp = DateTime.Now,
                }
            );
        }


    }
}
