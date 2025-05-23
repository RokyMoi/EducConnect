using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person;
using backend.Entities.Person;
using backend.Entities.Reference;
using backend.Interfaces.Person;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Person
{
    public class PersonCareerInformationRepository : IPersonCareerInformationRepository
    {

        private readonly DataContext _dataContext;

        public PersonCareerInformationRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<PersonCareerInformationDTO> AddPersonCareerInformation(PersonCareerInformationCreateDTO newPersonCareerInformation)
        {
            //Fetch Person 
            EduConnect.Entities.Person.Person person = await _dataContext.Person.Where(p => p.PersonId == newPersonCareerInformation.PersonId).FirstOrDefaultAsync();

            //Fetch EmploymentType 
            EmploymentType employmentType = await _dataContext.EmploymentType.Where(e => e.EmploymentTypeId == newPersonCareerInformation.EmploymentTypeId).FirstOrDefaultAsync();

            //Fetch WorkType, if in the newPersonCareerInformation parameter WorkTypeId is not null
            WorkType workType = null;
            if (newPersonCareerInformation.WorkTypeId != null)
            {
                workType = await _dataContext.WorkType.Where(x => x.WorkTypeId == newPersonCareerInformation.WorkTypeId).FirstOrDefaultAsync();
            }

            //Fetch IndustryClassification

            IndustryClassification industryClassification = await _dataContext.IndustryClassification.Where(i => i.IndustryClassificationId == newPersonCareerInformation.IndustryClassificationId).FirstOrDefaultAsync();

            //Create a new PersonCareerInformation object
            PersonCareerInformation personCareerInformation = new PersonCareerInformation()
            {
                PersonCareerInformationId = Guid.NewGuid(),
                PersonId = newPersonCareerInformation.PersonId,
                Person = person,
                CompanyName = newPersonCareerInformation.CompanyName,
                CompanyWebsite = newPersonCareerInformation.CompanyWebsite,
                JobTitle = newPersonCareerInformation.JobTitle,
                Position = newPersonCareerInformation.Position,
                CityOfEmployment = newPersonCareerInformation.CityOfEmployment,
                CountryOfEmployment = newPersonCareerInformation.CountryOfEmployment,
                EmploymentTypeId = newPersonCareerInformation.EmploymentTypeId,
                EmploymentType = employmentType,
                StartDate = newPersonCareerInformation.StartDate,
                EndDate = newPersonCareerInformation.EndDate,
                JobDescription = newPersonCareerInformation.JobDescription,
                Responsibilities = newPersonCareerInformation.Responsibilities,
                Achievements = newPersonCareerInformation.Achievements,
                IndustryClassificationId = newPersonCareerInformation.IndustryClassificationId,
                IndustryClassification = industryClassification,
                SkillsUsed = newPersonCareerInformation.SkillsUsed,
                WorkTypeId = newPersonCareerInformation.WorkTypeId,
                WorkType = workType,
                AdditionalInformation = newPersonCareerInformation.AdditionalInformation,
                CreatedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds(),

            };


            Console.WriteLine("StartDate: " + newPersonCareerInformation.StartDate);
            Console.WriteLine("EndDate: " + newPersonCareerInformation.EndDate);
            //Attempt to add the new PersonCareerInformation object to the database
            try
            {
                await _dataContext.PersonCareerInformation.AddAsync(personCareerInformation);
                await _dataContext.SaveChangesAsync();

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return null;
            }

            //Return PersonCareerInformationDTO object with the newly added data to the database from the PersonCareerInformation object 
            return new PersonCareerInformationDTO
            {
                PersonCareerInformationId = personCareerInformation.PersonCareerInformationId,
                PersonId = personCareerInformation.PersonId,
                CompanyName = personCareerInformation.CompanyName,
                CompanyWebsite = personCareerInformation.CompanyWebsite,
                JobTitle = personCareerInformation.JobTitle,
                Position = personCareerInformation.Position,
                CityOfEmployment = personCareerInformation.CityOfEmployment,
                CountryOfEmployment = personCareerInformation.CountryOfEmployment,
                EmploymentTypeId = personCareerInformation.EmploymentTypeId,
                StartDate = personCareerInformation.StartDate,
                EndDate = personCareerInformation.EndDate,
                JobDescription = personCareerInformation.JobDescription,
                Responsibilities = personCareerInformation.Responsibilities,
                Achievements = personCareerInformation.Achievements,
                IndustryClassificationId = personCareerInformation.IndustryClassificationId,
                SkillsUsed = personCareerInformation.SkillsUsed,
                WorkTypeId = personCareerInformation.WorkTypeId,
                AdditionalInformation = personCareerInformation.AdditionalInformation,

            };
        }

        public async Task<PersonCareerInformationDTO> DeletePersonCareerInformationById(Guid personCareerInformationId)
        {
            var personCareerInformation = await _dataContext.PersonCareerInformation.Where(p => p.PersonCareerInformationId == personCareerInformationId).FirstOrDefaultAsync();

            if (personCareerInformation == null)
            {
                return null;
            }

            try
            {
                _dataContext.PersonCareerInformation.Remove(personCareerInformation);
                await _dataContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error deleting person career information");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return null;

            }

            return new PersonCareerInformationDTO
            {
                PersonCareerInformationId = personCareerInformation.PersonCareerInformationId,
                PersonId = personCareerInformation.PersonId,
                CompanyName = personCareerInformation.CompanyName,
                CompanyWebsite = personCareerInformation.CompanyWebsite,
                JobTitle = personCareerInformation.JobTitle,
                Position = personCareerInformation.Position,
                CityOfEmployment = personCareerInformation.CityOfEmployment,
                CountryOfEmployment = personCareerInformation.CountryOfEmployment,
                EmploymentTypeId = personCareerInformation.EmploymentTypeId,
                StartDate = personCareerInformation.StartDate,
                EndDate = personCareerInformation.EndDate,
                JobDescription = personCareerInformation.JobDescription,
                Responsibilities = personCareerInformation.Responsibilities,
                Achievements = personCareerInformation.Achievements,
                IndustryClassificationId = personCareerInformation.IndustryClassificationId,
                SkillsUsed = personCareerInformation.SkillsUsed,
                WorkTypeId = personCareerInformation.WorkTypeId,
                AdditionalInformation = personCareerInformation.AdditionalInformation,
            };

        }

        public async Task<List<PersonCareerInformationDTO>> GetAllPersonCareerInformationByPersonId(Guid personId)
        {
            //Get all person career information by person id
            List<PersonCareerInformation> personCareerInformationList = await _dataContext.PersonCareerInformation.Include(x => x.IndustryClassification).Where(p => p.PersonId == personId).ToListAsync();

            //If there is no career information for the given person, return null
            if (personCareerInformationList == null)
            {
                return null;
            }


            //Create a new List of PersonCareerInformationDTO
            List<PersonCareerInformationDTO>
            careerInformationList = new List<PersonCareerInformationDTO>();

            foreach (PersonCareerInformation careerInformation in personCareerInformationList)
            {
                careerInformationList.Add(new PersonCareerInformationDTO
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
                    IndustryClassificationId = careerInformation.IndustryClassificationId,
                    Industry = careerInformation.IndustryClassification.Industry,
                    Sector = careerInformation.IndustryClassification.Sector,
                    SkillsUsed = careerInformation.SkillsUsed,
                    WorkTypeId = careerInformation.WorkTypeId,
                    AdditionalInformation = careerInformation.AdditionalInformation

                });
            };
            return careerInformationList;

        }

        public async Task<PersonCareerInformationDTO> GetPersonCareerInformationById(Guid personCareerInformationId)
        {
            var careerInformation = await _dataContext.PersonCareerInformation.Where(p => p.PersonCareerInformationId == personCareerInformationId).FirstOrDefaultAsync();

            if (careerInformation == null)
            {
                return null;
            }

            return new PersonCareerInformationDTO
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
                IndustryClassificationId = careerInformation.IndustryClassificationId,
                SkillsUsed = careerInformation.SkillsUsed,
                WorkTypeId = careerInformation.WorkTypeId,
                AdditionalInformation = careerInformation.AdditionalInformation
            };
        }

        public async Task<PersonCareerInformationDTO> UpdatePersonCareerInformation(PersonCareerInformationDTO updateDTO)
        {
            //Get the PersonCareerInformation to update from the database
            PersonCareerInformation personCareerInformation = await _dataContext.PersonCareerInformation.Where(p => p.PersonCareerInformationId == updateDTO.PersonCareerInformationId).FirstOrDefaultAsync();


            if (personCareerInformation == null)
            {
                return null;
            }

            //Convert the parameter PersonCareerInformationDTO to a PersonCareerInformation object

            personCareerInformation.PersonCareerInformationId = updateDTO.PersonCareerInformationId;
            personCareerInformation.PersonId = updateDTO.PersonId;
            personCareerInformation.CompanyName = updateDTO.CompanyName;
            personCareerInformation.CompanyWebsite = updateDTO.CompanyWebsite;
            personCareerInformation.JobTitle = updateDTO.JobTitle;
            personCareerInformation.Position = updateDTO.Position;
            personCareerInformation.CityOfEmployment = updateDTO.CityOfEmployment;
            personCareerInformation.CountryOfEmployment = updateDTO.CountryOfEmployment;
            personCareerInformation.EmploymentTypeId = updateDTO.EmploymentTypeId;
            personCareerInformation.StartDate = updateDTO.StartDate;
            personCareerInformation.EndDate = updateDTO.EndDate;
            personCareerInformation.JobDescription = updateDTO.JobDescription;
            personCareerInformation.Responsibilities = updateDTO.Responsibilities;
            personCareerInformation.Achievements = updateDTO.Achievements;
            personCareerInformation.IndustryClassificationId = updateDTO.IndustryClassificationId;
            personCareerInformation.SkillsUsed = updateDTO.SkillsUsed;
            personCareerInformation.WorkTypeId = updateDTO.WorkTypeId;
            personCareerInformation.AdditionalInformation = updateDTO.AdditionalInformation;
            personCareerInformation.UpdatedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();



            //Attempt to save the changes to the database
            try
            {
                await _dataContext.SaveChangesAsync();

            }
            catch (System.Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return null;
            }

            //Return the updated PersonCareerInformation object 
            return new PersonCareerInformationDTO
            {
                PersonCareerInformationId = personCareerInformation.PersonCareerInformationId,
                PersonId = personCareerInformation.PersonId,
                CompanyName = personCareerInformation.CompanyName,
                CompanyWebsite = personCareerInformation.CompanyWebsite,
                JobTitle = personCareerInformation.JobTitle,
                Position = personCareerInformation.Position,
                CityOfEmployment = personCareerInformation.CityOfEmployment,
                CountryOfEmployment = personCareerInformation.CountryOfEmployment,
                EmploymentTypeId = personCareerInformation.EmploymentTypeId,
                StartDate = personCareerInformation.StartDate,
                EndDate = personCareerInformation.EndDate,
                JobDescription = personCareerInformation.JobDescription,
                Responsibilities = personCareerInformation.Responsibilities,
                Achievements = personCareerInformation.Achievements,
                IndustryClassificationId = personCareerInformation.IndustryClassificationId,
                SkillsUsed = personCareerInformation.SkillsUsed,
                WorkTypeId = personCareerInformation.WorkTypeId,
                AdditionalInformation = personCareerInformation.AdditionalInformation,
            };
        }
    }
}