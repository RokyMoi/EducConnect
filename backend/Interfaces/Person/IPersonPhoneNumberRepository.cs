using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person.PersonPhoneNumber;

namespace backend.Interfaces.Person
{
    public interface IPersonPhoneNumberRepository
    {
        public Task<PersonPhoneNumberSaveDTO> CreatePersonPhoneNumber(PersonPhoneNumberSaveDTO personPhoneNumber);
        public Task<PersonPhoneNumberDTO?> GetPersonPhoneNumberByPersonId(Guid personId);
        public Task<PersonPhoneNumberDTO?> GetPersonPhoneNumberByPhoneNumber(string phoneNumber, Guid nationalCallingCodeCountryId);

        public Task<PersonPhoneNumberDTO?> UpdatePersonPhoneNumber(PersonPhoneNumberDTO personPhoneNumber);
        public Task<PersonPhoneNumberDTO?> DeletePersonPhoneNumberByPersonId(Guid personId);
    }
}