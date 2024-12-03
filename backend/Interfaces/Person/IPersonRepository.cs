using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person;
using backend.DTOs.Tutor;
using backend.Entities.Person;
using EduConnect.Entities.Person;

namespace backend.Interfaces.Person
{
    public interface IPersonRepository
    {
        Task<PersonEmailDTO> GetPersonEmailByEmail(string email);
        Task<PersonEmailPasswordSaltDTOGroup> CreateNewPersonWithHelperTables(EduConnect.Entities.Person.Person person, PersonEmail personEmail, PersonPassword personPassword, PersonSalt personSalt, PersonVerificationCode personVerificationCode);
        public Task<PersonVerificationCodeDTO> GetPersonVerificationCodeByEmail(string email);
        Task<PersonVerificationCodeDTO> VerifyPersonVerificationCode(PersonVerificationCodeDTO personVerificationCodeDTO);
        Task<PersonEmailWithPersonObjectDTO> GetPersonEmailWithPersonObjectByEmail(string email);
        Task<PersonVerificationCodeDTO> CreateNewPersonVerificationCode(PersonVerificationCode personVerificationCode);
        Task<PersonVerificationCode> DeletePersonVerificationCode(PersonVerificationCode personVerificationCode);
        Task<bool> DeletePersonVerificationCodeByPersonId(Guid personId);

        Task<TutorPersonDetailsDTO> GetTutorPersonInformationByPersonId(Guid personId);
        Task<TutorUsernameDTO> GetTutorByUsername(string username);
        Task<PersonDetailsDTO> CreateNewPersonDetails(PersonDetails newPersonDetails);

    }
}