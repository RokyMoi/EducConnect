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
            foreach (var personEducationInformationItem in personEducationInformation) { 
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
    }
}