using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person.PersonAvailability;

namespace backend.Interfaces.Person
{
    public interface IPersonAvailabilityRepository
    {
        Task<PersonAvailabilityDTO> AddPersonAvailability(PersonAvailabilityDTO personAvailabilityDTO);
        Task<PersonAvailabilityDTO> GetPersonAvailabilityById(Guid personAvailabilityId);
        Task<List<PersonAvailabilityDTO>> GetAllPersonAvailabilityByPersonId(Guid personId);
        Task<PersonAvailabilityDTO> DeletePersonAvailabilityById(Guid personAvailabilityId);


    }
}