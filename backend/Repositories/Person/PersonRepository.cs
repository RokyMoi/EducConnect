using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person;
using backend.DTOs.Person.PersonDetails;
using backend.DTOs.Person.PersonPhoneNumber;
using backend.DTOs.Tutor;
using backend.Entities.Person;
using backend.Interfaces.Person;
using EduConnect.Data;
using EduConnect.DTOs;
using EduConnect.Entities.Person;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Identity.Client;

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
            var personEmail = await _databaseContext.PersonEmail.Include(x => x.Person).Where(p => p.Email.Equals(email)).FirstOrDefaultAsync();

            if (personEmail == null)
            {
                return null;
            }

            return new PersonEmailDTO
            {
                PersonId = personEmail.PersonId,
                PersonEmailId = personEmail.PersonEmailId,
                Person = personEmail.Person,
                Email = personEmail.Email,


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

        public async Task<TutorPersonDetailsDTO> GetTutorPersonInformationByPersonId(Guid personId)
        {
            var tutorPersonalInformation = await _databaseContext.Tutor.Where(x => x.PersonId == personId)
            .GroupJoin(
                _databaseContext.PersonDetails,
                tutor => tutor.PersonId,
                personDetails => personDetails.PersonId,
                (tutor, personDetails) => new { tutor, personDetails }
            )
            .SelectMany(
                group => group.personDetails.DefaultIfEmpty(),
                (group, personDetails) => new
                {
                    Tutor = group.tutor,
                    PersonDetails = personDetails
                }
            )
            .FirstOrDefaultAsync();


            return new TutorPersonDetailsDTO
            {
                PersonDetailsId = tutorPersonalInformation.PersonDetails == null ? Guid.Empty : tutorPersonalInformation.PersonDetails.PersonDetailsId,
                PersonId = tutorPersonalInformation.Tutor.PersonId,
                TutorId = tutorPersonalInformation.Tutor.TutorId,
                FirstName = tutorPersonalInformation.PersonDetails == null ? string.Empty : tutorPersonalInformation.PersonDetails.FirstName,
                LastName = tutorPersonalInformation.PersonDetails == null ? string.Empty : tutorPersonalInformation.PersonDetails.LastName,
                Username = tutorPersonalInformation.PersonDetails == null ? string.Empty : tutorPersonalInformation.PersonDetails.Username,
                CountryOfOrigin = tutorPersonalInformation.PersonDetails?.CountryOfOriginCountryId,
            };

        }

        public async Task<TutorUsernameDTO> GetTutorByUsername(string username)
        {
            var tutorUsername = await _databaseContext.PersonDetails.Where(x => x.Username.Equals(username)).FirstOrDefaultAsync();

            if (tutorUsername == null)
            {
                return null;
            }

            var tutor = await _databaseContext.Tutor.Where(x => x.PersonId == tutorUsername.PersonId).FirstOrDefaultAsync();



            return new TutorUsernameDTO
            {
                PersonId = tutorUsername.PersonId,
                TutorId = tutor == null ? Guid.Empty : tutor.TutorId,
                PersonDetailsId = tutorUsername.PersonDetailsId,
                Username = tutorUsername.Username
            };


        }

        public async Task<PersonDetailsDTO> CreateNewPersonDetails(PersonDetails newPersonDetails)
        {
            try
            {
                await _databaseContext.PersonDetails.AddAsync(newPersonDetails);
                await _databaseContext.SaveChangesAsync();
                return new PersonDetailsDTO
                {
                    PersonDetailsId = newPersonDetails.PersonDetailsId,
                    PersonId = newPersonDetails.PersonId,
                    FirstName = newPersonDetails.FirstName,
                    LastName = newPersonDetails.LastName,
                    Username = newPersonDetails.Username,
                    CountryOfOriginCountryId = newPersonDetails.CountryOfOriginCountryId,
                };
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return null;


            }
        }

        public async Task<PersonDetailsDTO> GetPersonDetailsByPersonId(Guid personId)
        {
            var personDetails = await _databaseContext.PersonDetails.Where(x => x.PersonId == personId).FirstOrDefaultAsync();

            if (personDetails == null)
            {
                return null;
            }

            return new PersonDetailsDTO
            {
                PersonDetailsId = personDetails.PersonDetailsId,
                PersonId = personDetails.PersonId,
                FirstName = personDetails.FirstName,
                LastName = personDetails.LastName,
                Username = personDetails.Username,
                CountryOfOriginCountryId = personDetails.CountryOfOriginCountryId,
            };
        }


        public async Task<PersonDetailsDTO> GetPersonByUsername(string username)
        {
            var personDetails = await _databaseContext.PersonDetails.Where(x => x.Username.Equals(username)).FirstOrDefaultAsync();

            if (personDetails == null)
            {
                return null;
            }

            return new PersonDetailsDTO
            {
                PersonDetailsId = personDetails.PersonDetailsId,
                PersonId = personDetails.PersonId,
                FirstName = personDetails.FirstName,
                LastName = personDetails.LastName,
                Username = personDetails.Username,
                CountryOfOriginCountryId = personDetails.CountryOfOriginCountryId,
            };
        }

        public async Task<PersonDetailsDTO> UpdatePersonDetails(PersonDetailsUpdateDTO personDetails)
        {
            //Get the person details
            var personDetailsToUpdate = _databaseContext.PersonDetails.Where(x => x.PersonDetailsId == personDetails.PersonDetailsId).FirstOrDefault();

            if (personDetailsToUpdate == null)
            {
                return null;
            }

            //Update the person details
            personDetailsToUpdate.FirstName = personDetails.FirstName;
            personDetailsToUpdate.LastName = personDetails.LastName;
            personDetailsToUpdate.Username = personDetails.Username;
            personDetailsToUpdate.CountryOfOriginCountryId = personDetails.CountryOfOriginCountryId;
            personDetailsToUpdate.ModifiedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            await _databaseContext.SaveChangesAsync();
            return new PersonDetailsDTO
            {
                PersonDetailsId = personDetailsToUpdate.PersonDetailsId,
                PersonId = personDetailsToUpdate.PersonId,
                FirstName = personDetailsToUpdate.FirstName,
                LastName = personDetailsToUpdate.LastName,
                Username = personDetailsToUpdate.Username,
                CountryOfOriginCountryId = personDetailsToUpdate.CountryOfOriginCountryId,
            };
        }


        public async Task<PersonPhoneNumberDTO> CreateNewPersonPhoneNumber(PersonPhoneNumberSaveDTO personPhoneNumberDTO)
        {
            var newPersonPhoneNumber = new PersonPhoneNumber
            {
                PersonId = personPhoneNumberDTO.PersonId,
                PersonPhoneNumberId = Guid.NewGuid(),
                NationalCallingCodeCountryId = personPhoneNumberDTO.NationalCallingCodeCountryId,
                PhoneNumber = personPhoneNumberDTO.PhoneNumber,
            };

            try
            {
                await _databaseContext.PersonPhoneNumber.AddAsync(newPersonPhoneNumber);
                await _databaseContext.SaveChangesAsync();
                return new PersonPhoneNumberDTO
                {
                    PersonPhoneNumberId = newPersonPhoneNumber.PersonPhoneNumberId,
                    PersonId = newPersonPhoneNumber.PersonId,
                    NationalCallingCodeCountryId = newPersonPhoneNumber.NationalCallingCodeCountryId,
                    PhoneNumber = newPersonPhoneNumber.PhoneNumber,
                };
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error creating new person phone number");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return null;


            }


        }

        public async Task<PersonPhoneNumberDTO> GetPersonByPhoneNumber(Guid nationalCallingCodeCountryId, string phoneNumber)
        {

            var personPhoneNumber = await _databaseContext.PersonPhoneNumber.Where(x => x.NationalCallingCodeCountryId == nationalCallingCodeCountryId && x.PhoneNumber.Equals(phoneNumber)).FirstOrDefaultAsync();

            if (personPhoneNumber == null)
            {
                return null;
            }

            return new PersonPhoneNumberDTO
            {

                PersonPhoneNumberId = personPhoneNumber.PersonPhoneNumberId,
                PersonId = personPhoneNumber.PersonId,
                NationalCallingCodeCountryId = personPhoneNumber.NationalCallingCodeCountryId,
                PhoneNumber = personPhoneNumber.PhoneNumber,
            };
        }

        public async Task<PersonPhoneNumberDTO?> GetPersonPhoneNumberByPersonId(Guid personId)
        {
            var personPhoneNumber = await _databaseContext.PersonPhoneNumber.Where(x => x.PersonId == personId).FirstOrDefaultAsync();

            if (personPhoneNumber == null)
            {
                return null;
            }

            return new PersonPhoneNumberDTO
            {
                PersonId = personPhoneNumber.PersonId,
                PersonPhoneNumberId = personPhoneNumber.PersonPhoneNumberId,
                NationalCallingCodeCountryId = personPhoneNumber.NationalCallingCodeCountryId,
                PhoneNumber = personPhoneNumber.PhoneNumber,
            };
        }

        public async Task<List<IdentityRole<Guid>>?> GetRolesByPersonId(Guid personId)
        {
            var roleIds = await _databaseContext.UserRoles.Where(x => x.UserId == personId).Select(x => x.RoleId).ToListAsync();

            return await _databaseContext.Roles.Where(x => roleIds.Contains(x.Id)).ToListAsync();

        }

        public Task<EduConnect.Entities.Person.Person?> GetPersonByEmailOrUsername(string emailOrUsername)
        {
            return
            _databaseContext
            .Person
            .Include(x => x.PersonDetails)
            .Include(x => x.PersonEmail)
            .Where(x => x.PersonEmail.Email.Equals(emailOrUsername.Trim()) || x.PersonDetails.Username.Equals(emailOrUsername.TrimEnd())).FirstOrDefaultAsync();



        }


        public async Task<bool> EmailExists(string email)
        {
            return await _databaseContext.PersonEmail.Where(x => x.Email.Equals(email.Trim())).AnyAsync();
        }

        public async Task<bool> CreatePerson(EduConnect.Entities.Person.Person person)
        {
            try
            {
                await _databaseContext.Person.AddAsync(person);
                await _databaseContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error creating new person");
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> CreatePersonEmail(PersonEmail personEmail)
        {
            try
            {
                await _databaseContext.PersonEmail.AddAsync(personEmail);
                await _databaseContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error creating new person email");
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> CreatePersonDetails(PersonDetails personDetails)
        {
            try
            {
                await _databaseContext.PersonDetails.AddAsync(personDetails);
                await _databaseContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error creating new person details");
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> CreatePersonPhoneNumber(PersonPhoneNumber personPhoneNumber)
        {
            try
            {
                await _databaseContext.PersonPhoneNumber.AddAsync(personPhoneNumber);
                await _databaseContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error creating new person phone number");
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> CreatePersonPassword(PersonPassword personPassword)
        {
            try
            {
                await _databaseContext.PersonPassword.AddAsync(personPassword);
                await _databaseContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error creating new person password");
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<PersonPassword?> GetPersonPasswordByPersonId(Guid personId)
        {
            return await _databaseContext.PersonPassword.Where(x => x.PersonId == personId).FirstOrDefaultAsync();
        }

        public async Task<EduConnect.Entities.Person.Person?> GetPersonByPublicPersonId(Guid publicPersonId)
        {
            return await _databaseContext.Person.Where(x => x.PersonPublicId == publicPersonId).FirstOrDefaultAsync();
        }

        public async Task<GetDashboardPersonInfoResponse?> GetDashboardPersonInfo(Guid personId)
        {
            return await _databaseContext
            .Person
            .Include(x => x.PersonDetails)
            .Include(x => x.PersonEmail)
            .Where(x => x.PersonId == personId)
            .Select(
                x => new GetDashboardPersonInfoResponse
                {
                    Email = x.PersonEmail.Email,
                    FirstName = x.PersonDetails.FirstName,
                    LastName = x.PersonDetails.LastName,
                    Username = x.PersonDetails.Username,
                }
            )
            .FirstOrDefaultAsync();

        }
    }
}





