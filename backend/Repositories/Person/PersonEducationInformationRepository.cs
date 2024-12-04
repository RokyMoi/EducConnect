using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person;
using backend.Entities.Person;
using backend.Interfaces.Person;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Person
{
    public class PersonEducationInformationRepository : IPersonEducationInformationRepository
    {
        private readonly DataContext _databaseContext;

        public PersonEducationInformationRepository(DataContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }

        public async Task<PersonEducationInformationDTO> AddPersonEducationInformation(PersonEducationInformation newPersonEducationInformation)
        {
            try
            {
                await _databaseContext.PersonEducationInformation.AddAsync(newPersonEducationInformation);
                await _databaseContext.SaveChangesAsync();
                return new PersonEducationInformationDTO
                {
                    PersonEducationInformationId = newPersonEducationInformation.PersonEducationInformationId,
                    PersonId = newPersonEducationInformation.PersonId,
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
                    Description = newPersonEducationInformation.Description
                };


            }
            catch (System.Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return null;
            }
        }

        public async Task<List<PersonEducationInformationDTO>> GetAllPersonEducationInformationByPersonId(Guid personId)
        {
            var personEducationInformation = await _databaseContext.PersonEducationInformation.Where(x => x.PersonId == personId).ToListAsync();

            if (personEducationInformation == null || !personEducationInformation.Any())
            {
                return null;
            }

            List<PersonEducationInformationDTO> personEducationInformationList = new List<PersonEducationInformationDTO>();
            foreach (var personEducationInformationItem in personEducationInformation)
            {
                personEducationInformationList.Add(new PersonEducationInformationDTO
                {
                    PersonEducationInformationId = personEducationInformationItem.PersonEducationInformationId,
                    PersonId = personEducationInformationItem.PersonId,
                    InstitutionName = personEducationInformationItem.InstitutionName,
                    InstitutionOfficialWebsite = personEducationInformationItem.InstitutionOfficialWebsite,
                    InstitutionAddress = personEducationInformationItem.InstitutionAddress,
                    EducationLevel = personEducationInformationItem.EducationLevel,
                    FieldOfStudy = personEducationInformationItem.FieldOfStudy,
                    MinorFieldOfStudy = personEducationInformationItem.MinorFieldOfStudy,
                    StartDate = personEducationInformationItem.StartDate,
                    EndDate = personEducationInformationItem.EndDate,
                    IsCompleted = personEducationInformationItem.IsCompleted,
                    FinalGrade = personEducationInformationItem.FinalGrade,
                    Description = personEducationInformationItem.Description
                });
            }
            return personEducationInformationList;
        }

        public async Task<PersonEducationInformationDTO> GetPersonEducationInformationById(Guid personEducationInformationId)
        {
            var personEducationInformation = await _databaseContext.PersonEducationInformation.Where(x => x.PersonEducationInformationId == personEducationInformationId).FirstOrDefaultAsync();
            if (personEducationInformation == null)
            {
                return null;
            }
            return new PersonEducationInformationDTO
            {
                PersonEducationInformationId = personEducationInformation.PersonEducationInformationId,
                PersonId = personEducationInformation.PersonId,
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
                Description = personEducationInformation.Description
            };

        }

        public async Task<PersonEducationInformationDTO> UpdatePersonEducationInformation(PersonEducationInformationUpdateDTO updatedPersonEducationInformation)
        {
            try
            {
                //Get the PersonEducationInformation object to update
                var personEducationInformationToUpdate = await _databaseContext.PersonEducationInformation.Where(x => x.PersonEducationInformationId == updatedPersonEducationInformation.PersonEducationInformationId).FirstOrDefaultAsync();

                //Check if the PersonEducationInformation object exists
                if (personEducationInformationToUpdate == null)
                {
                    return null;
                }

                //Check if the existing values are different from the new values

                //Set a flag variable to check if any value has been updated
                bool isUpdated = false;
                //Check InstitutionName
                if (personEducationInformationToUpdate.InstitutionName != updatedPersonEducationInformation.InstitutionName)
                {
                    personEducationInformationToUpdate.InstitutionName = updatedPersonEducationInformation.InstitutionName;
                    isUpdated = true;
                }

                //Check InstitutionOfficialWebsite
                if (personEducationInformationToUpdate.InstitutionOfficialWebsite != updatedPersonEducationInformation.InstitutionOfficialWebsite)
                {
                    personEducationInformationToUpdate.InstitutionOfficialWebsite = updatedPersonEducationInformation.InstitutionOfficialWebsite;
                    isUpdated = true;
                }

                //Check InstitutionAddress
                if (personEducationInformationToUpdate.InstitutionAddress != updatedPersonEducationInformation.InstitutionAddress)
                {
                    personEducationInformationToUpdate.InstitutionAddress = updatedPersonEducationInformation.InstitutionAddress;
                    isUpdated = true;
                }

                //Check EducationLevel
                if (updatedPersonEducationInformation.EducationLevel != null && personEducationInformationToUpdate.EducationLevel != updatedPersonEducationInformation.EducationLevel)
                {
                    personEducationInformationToUpdate.EducationLevel = updatedPersonEducationInformation.EducationLevel;
                    isUpdated = true;
                }

                //Check FieldOfStudy
                if (updatedPersonEducationInformation.FieldOfStudy != null && personEducationInformationToUpdate.FieldOfStudy != updatedPersonEducationInformation.FieldOfStudy)
                {
                    personEducationInformationToUpdate.FieldOfStudy = updatedPersonEducationInformation.FieldOfStudy;
                    isUpdated = true;
                }

                //Check MinorFieldOfStudy
                if (personEducationInformationToUpdate.MinorFieldOfStudy != updatedPersonEducationInformation.MinorFieldOfStudy)
                {
                    personEducationInformationToUpdate.MinorFieldOfStudy = updatedPersonEducationInformation.MinorFieldOfStudy;
                    isUpdated = true;
                }

                //Check StartDate
                if (personEducationInformationToUpdate.StartDate != updatedPersonEducationInformation.StartDate)
                {
                    personEducationInformationToUpdate.StartDate = updatedPersonEducationInformation.StartDate;
                    isUpdated = true;
                }
                //Check EndDate
                if (personEducationInformationToUpdate.EndDate != updatedPersonEducationInformation.EndDate)
                {
                    personEducationInformationToUpdate.EndDate = updatedPersonEducationInformation.EndDate;
                    isUpdated = true;
                }
                //Check IsCompleted
                if (updatedPersonEducationInformation.IsCompleted.HasValue && personEducationInformationToUpdate.IsCompleted != updatedPersonEducationInformation.IsCompleted)
                {
                    personEducationInformationToUpdate.IsCompleted = updatedPersonEducationInformation.IsCompleted.Value;
                    isUpdated = true;
                }

                //Check FinalGrade 
                if (updatedPersonEducationInformation.FinalGrade != null && personEducationInformationToUpdate.FinalGrade != updatedPersonEducationInformation.FinalGrade)
                {
                    personEducationInformationToUpdate.FinalGrade = updatedPersonEducationInformation.FinalGrade;
                    isUpdated = true;
                }

                //Check Description
                if (updatedPersonEducationInformation.Description != null && personEducationInformationToUpdate.Description != updatedPersonEducationInformation.Description)
                {
                    personEducationInformationToUpdate.Description = updatedPersonEducationInformation.Description;
                    isUpdated = true;
                }

                //Check if the isUpdated flag is false, if so, return null
                if (!isUpdated)
                {
                    return null;
                }
                personEducationInformationToUpdate.ModifiedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                //Save the new values to the database
                await _databaseContext.SaveChangesAsync();
                return new PersonEducationInformationDTO
                {
                    PersonEducationInformationId = personEducationInformationToUpdate.PersonEducationInformationId,
                    PersonId = personEducationInformationToUpdate.PersonId,
                    InstitutionName = personEducationInformationToUpdate.InstitutionName,
                    InstitutionOfficialWebsite = personEducationInformationToUpdate.InstitutionOfficialWebsite,
                    InstitutionAddress = personEducationInformationToUpdate.InstitutionAddress,
                    EducationLevel = personEducationInformationToUpdate.EducationLevel,
                    FieldOfStudy = personEducationInformationToUpdate.FieldOfStudy,
                    MinorFieldOfStudy = personEducationInformationToUpdate.MinorFieldOfStudy,
                    StartDate = personEducationInformationToUpdate.StartDate,
                    EndDate = personEducationInformationToUpdate.EndDate,
                    IsCompleted = personEducationInformationToUpdate.IsCompleted,
                    FinalGrade = personEducationInformationToUpdate.FinalGrade,
                    Description = personEducationInformationToUpdate.Description
                };
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error updating person education information: " + ex.Message + "\n" + ex.InnerException);
                return null;
            }
        }
    }
}