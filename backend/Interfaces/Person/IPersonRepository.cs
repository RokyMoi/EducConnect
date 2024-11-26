using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person;
using backend.Entities.Person;
using EduConnect.Entities.Person;

namespace backend.Interfaces.Person
{
    public interface IPersonRepository
    {
        Task<PersonEmailDTO> GetPersonIdByEmail(string email);
        Task<PersonEmailPasswordSaltDTOGroup> CreateNewPersonWithHelperTables(EduConnect.Entities.Person.Person person, PersonEmail personEmail, PersonPassword personPassword, PersonSalt personSalt, PersonVerificationCode personVerificationCode);
        Task<PersonVerificationCodeDTO> GetPersonVerificationCodeByEmail(string email);
        Task<PersonVerificationCodeDTO> VerifyPersonVerificationCode(PersonVerificationCodeDTO personVerificationCodeDTO);

    }
}