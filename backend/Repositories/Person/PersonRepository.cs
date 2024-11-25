using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person;
using backend.Entities.Person;
using backend.Interfaces.Person;
using EduConnect.Data;
using EduConnect.Entities.Person;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Person
{
    public class PersonRepository : Interfaces.Person.IPersonRepository
    {

        private readonly DataContext _databaseContext;

        public PersonRepository(DataContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }



        public async Task<PersonEmailDTO> GetPersonIdByEmail(string email)
        {
            var personEmail = await _databaseContext.PersonEmail.Where(p => p.Email.Equals(email)).FirstOrDefaultAsync();

            if (personEmail == null)
            {
                return null;
            }

            return new PersonEmailDTO
            {
                PersonId = personEmail.PersonId.ToString(),
                PersonEmailId = personEmail.PersonEmailId.ToString(),
                Email = personEmail.Email,


            };


        }
        public async Task<PersonEmailPasswordSaltDTOGroup> CreateNewPersonWithHelperTables(EduConnect.Entities.Person.Person person, PersonEmail personEmail, PersonPassword personPassword, PersonSalt personSalt, PersonVerificationCode personVerificationCode)
        {

            try
            {

                await _databaseContext.Person.AddAsync(person);
                await _databaseContext.PersonEmail.AddAsync(personEmail);
                await _databaseContext.PersonPassword.AddAsync(personPassword);
                await _databaseContext.PersonSalt.AddAsync(personSalt);
                await _databaseContext.PersonVerificationCode.AddAsync(personVerificationCode);
                await _databaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var personDTO = new PersonDTO
            {
                PersonId = person.PersonId.ToString(),
                IsActive = person.IsActive,

            };

            var personEmailDTO = new PersonEmailDTO
            {
                PersonId = personEmail.PersonId.ToString(),
                PersonEmailId = personEmail.PersonEmailId.ToString(),
                Email = personEmail.Email,
            };

            var personPasswordDTO = new PersonPasswordDTO
            {
                PersonPasswordId = personPassword.PersonPasswordId,
                PersonId = personPassword.PersonId,
                Hash = personPassword.Hash,
                Salt = personPassword.Salt,
            };

            var personSaltDTO = new PersonSaltDTO
            {
                PersonSaltId = personSalt.PersonSaltId,
                PersonId = personSalt.PersonId,
                Salt = personSalt.Salt,
            };

            var personVerificationCodeDTO = new PersonVerificationCodeDTO
            {
                PersonVerificationCodeId = personVerificationCode.PersonVerificationCodeId,
                PersonId = personVerificationCode.PersonId,
                VerificationCode = personVerificationCode.VerificationCode,
                ExpiryDateTime = personVerificationCode.ExpiryDateTime,
                IsVerified = personVerificationCode.IsVerified,

            };

            return new PersonEmailPasswordSaltDTOGroup
            {
                PersonDTO = personDTO,
                PersonEmailDTO = personEmailDTO,
                PersonPasswordDTO = personPasswordDTO,
                PersonSaltDTO = personSaltDTO,
                PersonVerificationCodeDTO = personVerificationCodeDTO,
            };
        }


    }


}
