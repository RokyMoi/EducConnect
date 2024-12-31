using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person.PersonPhoneNumber;
using backend.Entities.Person;
using backend.Interfaces.Person;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Person
{
    public class PersonPhoneNumberRepository(DataContext _dataContext) : IPersonPhoneNumberRepository
    {
        public async Task<PersonPhoneNumberSaveDTO> CreatePersonPhoneNumber(PersonPhoneNumberSaveDTO personPhoneNumber)
        {
            PersonPhoneNumber newPersonPhoneNumber = new PersonPhoneNumber
            {
                PersonPhoneNumberId = Guid.NewGuid(),
                PersonId = personPhoneNumber.PersonId,
                NationalCallingCodeCountryId = personPhoneNumber.NationalCallingCodeCountryId,
                PhoneNumber = personPhoneNumber.PhoneNumber,
                CreatedAt = new DateTimeOffset().ToUnixTimeMilliseconds()
            };

            try
            {
                await _dataContext.PersonPhoneNumber.AddAsync(
                    newPersonPhoneNumber
                );
                await _dataContext.SaveChangesAsync();
                return new PersonPhoneNumberSaveDTO
                {
                    PersonPhoneNumberId = newPersonPhoneNumber.PersonPhoneNumberId,
                    PersonId = newPersonPhoneNumber.PersonId,
                    NationalCallingCodeCountryId = newPersonPhoneNumber.NationalCallingCodeCountryId,
                    PhoneNumber = newPersonPhoneNumber.PhoneNumber,

                };
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error creating person phone number");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return null;
            }

        }

        public async Task<PersonPhoneNumberDTO?> DeletePersonPhoneNumberByPersonId(Guid personId)
        {
            var personPhoneNumber = await _dataContext.PersonPhoneNumber.Include(x => x.Country).Where(x => x.PersonId == personId).FirstOrDefaultAsync();

            if (personPhoneNumber == null)
            {
                return null;
            }

            _dataContext.PersonPhoneNumber.Remove(personPhoneNumber);
            await _dataContext.SaveChangesAsync();
            return new PersonPhoneNumberDTO
            {
                PersonPhoneNumberId = personPhoneNumber.PersonPhoneNumberId,
                PersonId = personPhoneNumber.PersonId,
                NationalCallingCodeCountryId = personPhoneNumber.NationalCallingCodeCountryId,
                NationalCallingCode = personPhoneNumber.Country.NationalCallingCode,
                NationalCallingCodeCountryName = personPhoneNumber.Country.Name,
                PhoneNumber = personPhoneNumber.PhoneNumber,
            };
        }


        public async Task<PersonPhoneNumberDTO?> GetPersonPhoneNumberByPersonId(Guid personId)
        {
            var personPhoneNumber = await _dataContext.PersonPhoneNumber.Include(x => x.Country).Where(x => x.PersonId == personId).FirstOrDefaultAsync();
            if (personPhoneNumber == null)
            {
                return null;
            }

            return new PersonPhoneNumberDTO
            {
                PersonPhoneNumberId = personPhoneNumber.PersonPhoneNumberId,
                PersonId = personPhoneNumber.PersonId,
                NationalCallingCodeCountryId = personPhoneNumber.NationalCallingCodeCountryId,
                NationalCallingCode = personPhoneNumber.Country.NationalCallingCode,
                NationalCallingCodeCountryName = personPhoneNumber.Country.Name,
                PhoneNumber = personPhoneNumber.PhoneNumber,

            };
        }

        public async Task<PersonPhoneNumberDTO?> GetPersonPhoneNumberByPhoneNumber(string phoneNumber, Guid nationalCallingCodeCountryId)
        {
            var personPhoneNumber = await _dataContext.PersonPhoneNumber.Include(x => x.Country).Where(x => x.PhoneNumber == phoneNumber && x.NationalCallingCodeCountryId == nationalCallingCodeCountryId).FirstOrDefaultAsync();
            if (personPhoneNumber == null)
            {
                return null;
            }
            return new PersonPhoneNumberDTO
            {
                PersonPhoneNumberId = personPhoneNumber.PersonPhoneNumberId,
                PersonId = personPhoneNumber.PersonId,
                NationalCallingCodeCountryId = personPhoneNumber.NationalCallingCodeCountryId,
                NationalCallingCode = personPhoneNumber.Country.NationalCallingCode,
                NationalCallingCodeCountryName = personPhoneNumber.Country.Name,

                PhoneNumber = personPhoneNumber.PhoneNumber,
            };
        }

        public async Task<PersonPhoneNumberDTO?> UpdatePersonPhoneNumber(PersonPhoneNumberDTO personPhoneNumber)
        {
            var personPhoneNumberToUpdate = _dataContext.PersonPhoneNumber.Where(x => x.PersonPhoneNumberId == personPhoneNumber.PersonPhoneNumberId).FirstOrDefault();

            if (personPhoneNumberToUpdate == null)
            {
                return null;
            }

            personPhoneNumberToUpdate.PhoneNumber = personPhoneNumber.PhoneNumber;
            personPhoneNumberToUpdate.NationalCallingCodeCountryId = personPhoneNumber.NationalCallingCodeCountryId;
            personPhoneNumberToUpdate.UpdatedAt = new DateTimeOffset().ToUnixTimeMilliseconds();

            try
            {
                await _dataContext.SaveChangesAsync();
                return personPhoneNumber;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error updating person phone number");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return null;

            }
        }
    }
}
