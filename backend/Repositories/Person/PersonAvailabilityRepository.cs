using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Person.PersonAvailability;
using backend.Entities.Person;
using backend.Interfaces.Person;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Person
{
    public class PersonAvailabilityRepository : IPersonAvailabilityRepository
    {

        private readonly DataContext _dataContext;

        public PersonAvailabilityRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<PersonAvailabilityDTO> AddPersonAvailability(PersonAvailabilityDTO personAvailabilityDTO)
        {

            //Create new PersonAvailability object and assign the values to the object from parameter PersonAvailabilityDTO

            var personAvailability = new PersonAvailability
            {
                PersonAvailabilityId = Guid.NewGuid(),
                PersonId = personAvailabilityDTO.PersonId,
                DayOfWeek = personAvailabilityDTO.DayOfWeek,
                StartTime = personAvailabilityDTO.StartTime,
                EndTime = personAvailabilityDTO.EndTime,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            };

            //Attempt to add the PersonAvailability object to the database
            try
            {
                await _dataContext.PersonAvailibility.AddAsync(personAvailability);

                await _dataContext.SaveChangesAsync();

                personAvailabilityDTO.PersonAvailabilityId = personAvailability.PersonAvailabilityId;

            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error adding person availability");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return null;
            }
            return personAvailabilityDTO;
        }

        public async Task<PersonAvailabilityDTO> DeletePersonAvailabilityById(Guid personAvailabilityId)
        {
            var personAvailability = await _dataContext.PersonAvailibility.Where(x => x.PersonAvailabilityId == personAvailabilityId).FirstOrDefaultAsync();

            if (personAvailability == null)
            {
                return null;
            }

            _dataContext.PersonAvailibility.Remove(personAvailability);
            await _dataContext.SaveChangesAsync();

            var personAvailabilityDTO = new PersonAvailabilityDTO
            {
                PersonAvailabilityId = personAvailability.PersonAvailabilityId,
                PersonId = personAvailability.PersonId,
                DayOfWeek = personAvailability.DayOfWeek,
                StartTime = personAvailability.StartTime,
                EndTime = personAvailability.EndTime,
            };

            return personAvailabilityDTO;
        }



        public async Task<List<PersonAvailabilityDTO>> GetAllPersonAvailabilityByPersonId(Guid personId)
        {
            var personAvailabilityList = await _dataContext.PersonAvailibility.Where(p => p.PersonId == personId).ToListAsync();

            if (personAvailabilityList == null || personAvailabilityList.Count == 0)
            {
                return null;
            }

            var personAvailabilityDTOList = new List<PersonAvailabilityDTO>();

            foreach (var personAvailability in personAvailabilityList)
            {
                personAvailabilityDTOList.Add(
                    new PersonAvailabilityDTO
                    {
                        PersonAvailabilityId = personAvailability.PersonAvailabilityId,
                        PersonId = personAvailability.PersonId,
                        DayOfWeek = personAvailability.DayOfWeek,
                        StartTime = personAvailability.StartTime,
                        EndTime = personAvailability.EndTime,
                    }
                );
            }

            return personAvailabilityDTOList;
        }

        public async Task<PersonAvailabilityDTO> GetPersonAvailabilityById(Guid personAvailabilityId)
        {
            var personAvailability = await _dataContext.PersonAvailibility.Where(p => p.PersonAvailabilityId == personAvailabilityId).FirstOrDefaultAsync();

            if (personAvailability == null)
            {
                return null;
            }

            return new PersonAvailabilityDTO
            {
                PersonAvailabilityId = personAvailability.PersonAvailabilityId,
                PersonId = personAvailability.PersonId,
                DayOfWeek = personAvailability.DayOfWeek,
                StartTime = personAvailability.StartTime,
                EndTime = personAvailability.EndTime,

            };
        }


        /// <summary>
        /// Checks if the given time interval is available for the specified person on the given day of the week.
        /// </summary>
        /// <param name="personId">The ID of the person to check availability for.</param>
        /// <param name="dayOfWeek">The day of the week to check availability for.</param>
        /// <param name="startTime">The start time of the interval to check.</param>
        /// <param name="endTime">The end time of the interval to check.</param>
        /// <returns>
        /// <c>true</c> if the interval is available, <c>false</c> if the interval is not available, or <c>null</c> if the end time is less than or equal to the start time.
        /// </returns>
        public async Task<bool?> IsIntervalAvailable(Guid personId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime <= startTime)
            {
                return null;
            }


            var isOverlapping = await _dataContext.PersonAvailibility
            .Where(x => x.PersonId == personId && x.DayOfWeek == dayOfWeek)
            .AnyAsync(x => startTime < x.EndTime && endTime > x.StartTime);

            return isOverlapping;
        }


        public async Task<PersonAvailabilityDTO> UpdatePersonAvailabilityById(PersonAvailabilityDTO updateDTO)
        {
            var personAvailability = await _dataContext.PersonAvailibility.Where(p => p.PersonAvailabilityId == updateDTO.PersonAvailabilityId).FirstOrDefaultAsync();

            if (personAvailability == null)
            {
                return null;
            }

            personAvailability.StartTime = updateDTO.StartTime;
            personAvailability.EndTime = updateDTO.EndTime;
            personAvailability.DayOfWeek = updateDTO.DayOfWeek;
            personAvailability.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();


            try
            {
                await _dataContext.SaveChangesAsync();

            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error updating person availability");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return null;
            }
            return updateDTO;
        }
    }
}