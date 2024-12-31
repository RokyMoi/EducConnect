using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person;

namespace backend.Interfaces.Person
{
    public interface IPersonCareerInformationRepository
    {
        Task<List<PersonCareerInformationDTO>> GetAllPersonCareerInformationByPersonId(Guid personId);

        Task<PersonCareerInformationDTO>
        AddPersonCareerInformation(PersonCareerInformationCreateDTO newPersonCareerInformation);

        Task<PersonCareerInformationDTO> GetPersonCareerInformationById(Guid personCareerInformationId);

        Task<PersonCareerInformationDTO> UpdatePersonCareerInformation(PersonCareerInformationDTO updateDTO);

        Task<PersonCareerInformationDTO> DeletePersonCareerInformationById(Guid personCareerInformationId);
    }
}