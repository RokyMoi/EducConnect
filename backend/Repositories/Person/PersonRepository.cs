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



        public async Task<PersonEmailDTO> GetPersonEmailByEmail(string email)
        {
            var personEmail = await _databaseContext.PersonEmail.Where(p => p.Email.Equals(email)).FirstOrDefaultAsync();

            if (personEmail == null)
            {
                return null;
            }

            return new PersonEmailDTO
            {
                PersonId = personEmail.PersonId,
                PersonEmailId = personEmail.PersonEmailId,
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
                PersonId = personEmail.PersonId,
                PersonEmailId = personEmail.PersonEmailId,
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





        public async Task<PersonVerificationCodeDTO> GetPersonVerificationCodeByEmail(string email)
        {

            var personVerificationCode = await _databaseContext.PersonVerificationCode.Join(
                            _databaseContext.PersonEmail,
                            pvc => pvc.PersonId,
                            pe => pe.PersonId,
                            (pvc, pe) => new { PersonVerificationCode = pvc, pe.Email }
                        )
                        .Where(x => x.Email == email)
                        .Select(x => x.PersonVerificationCode)
                        .FirstOrDefaultAsync();

            if (personVerificationCode == null)
            {
                return null;
            }

            return new PersonVerificationCodeDTO
            {
                PersonVerificationCodeId = personVerificationCode.PersonVerificationCodeId,
                PersonId = personVerificationCode.PersonId,
                VerificationCode = personVerificationCode.VerificationCode,
                ExpiryDateTime = personVerificationCode.ExpiryDateTime,
                IsVerified = personVerificationCode.IsVerified,
            };
        }

        public async Task<PersonVerificationCodeDTO> VerifyPersonVerificationCode(PersonVerificationCodeDTO personVerificationCodeDTO)
        {
            var personVerificationCode = await _databaseContext.PersonVerificationCode.FindAsync(personVerificationCodeDTO.PersonVerificationCodeId);
            if (personVerificationCode == null)
            {
                return null;
            }

            personVerificationCode.IsVerified = true;
            personVerificationCode.ModifiedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            await _databaseContext.SaveChangesAsync();
            return new PersonVerificationCodeDTO
            {
                PersonVerificationCodeId = personVerificationCode.PersonVerificationCodeId,
                PersonId = personVerificationCode.PersonId,
                VerificationCode = personVerificationCode.VerificationCode,
                ExpiryDateTime = personVerificationCode.ExpiryDateTime,
                IsVerified = personVerificationCode.IsVerified,
            };

        }


        public async Task<PersonVerificationCodeDTO> CreateNewPersonVerificationCode(PersonVerificationCode personVerificationCode)
        {
            //Attempt to save a PersonVerificationCode object to the database provided within this functions' parameters.
            try
            {
                await _databaseContext.PersonVerificationCode.AddAsync(personVerificationCode);
                await _databaseContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;
            }

            return new PersonVerificationCodeDTO
            {
                PersonVerificationCodeId = personVerificationCode.PersonVerificationCodeId,
                PersonId = personVerificationCode.PersonId,
                VerificationCode = personVerificationCode.VerificationCode,
                ExpiryDateTime = personVerificationCode.ExpiryDateTime,
                IsVerified = personVerificationCode.IsVerified,
            };
        }

        public async Task<PersonVerificationCode> DeletePersonVerificationCode(PersonVerificationCode personVerificationCode)
        {
            try
            {

                _databaseContext.PersonVerificationCode.Remove(personVerificationCode);
                await _databaseContext.SaveChangesAsync();
                return personVerificationCode;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error deleting person verification code: " + ex.Message);
                return null;
            }
        }


        public async Task<bool> DeletePersonVerificationCodeByPersonId(Guid personId)
        {
            try
            {
                var personVerificationCode = await _databaseContext.PersonVerificationCode.Where(x => x.PersonId == personId).FirstOrDefaultAsync();
                if (personVerificationCode == null)
                {
                    return false;
                }

                _databaseContext.PersonVerificationCode.Remove(personVerificationCode);
                await _databaseContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error deleting person verification code: " + ex.Message);
                return false;
            }
        }

        public async Task<PersonEmailWithPersonObjectDTO> GetPersonEmailWithPersonObjectByEmail(string email)
        {
            var personEmail = await _databaseContext
            .PersonEmail
            .Include(x => x.Person)
            .Where(x => x.Email == email)
            .FirstOrDefaultAsync();

            if (personEmail == null)
            {
                return null;
            }

            return new PersonEmailWithPersonObjectDTO
            {
                PersonEmailId = personEmail.PersonEmailId,
                PersonId = personEmail.PersonId,
                Person = personEmail.Person,
                Email = personEmail.Email,
            };
        }
    }
}



