using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person;
using backend.Entities.Person;

namespace backend.Interfaces.Person
{
    public interface IPersonEducationInformationRepository
    {
        Task<List<PersonEducationInformationDTO>> GetAllPersonEducationInformationByPersonId(Guid personId);
        Task<PersonEducationInformationDTO> AddPersonEducationInformation(PersonEducationInformation newPersonEducationInformation);
        Task<PersonEducationInformationDTO> UpdatePersonEducationInformation(PersonEducationInformationUpdateDTO updatedPersonEducationInformation);
        Task<PersonEducationInformationDTO> GetPersonEducationInformationById(Guid personEducationInformationId);       

    }
}