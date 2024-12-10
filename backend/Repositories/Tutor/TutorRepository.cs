using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Tutor;
using backend.Interfaces.Tutor;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Tutor
{
    public class TutorRepository : ITutorRepository
    {
        private readonly DataContext _databaseContext;

        public TutorRepository(DataContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public async Task<TutorDTO> CreateTutor(EduConnect.Entities.Tutor.Tutor tutor)
        {
            await _databaseContext.Tutor.AddAsync(tutor);
            await _databaseContext.SaveChangesAsync();
            return new TutorDTO
            {
                TutorId = tutor.TutorId,
                PersonId = tutor.PersonId
            };

        }

        public async Task<TutorDTO> GetTutorByPersonId(Guid personId)
        {
            var tutor = await _databaseContext.Tutor.Include(x => x.TutorRegistrationStatus).Where(x => x.PersonId == personId).FirstOrDefaultAsync();

            if (tutor == null)
            {
                return null;
            }

            return new TutorDTO
            {
                TutorId = tutor.TutorId,
                PersonId = tutor.PersonId,
            };
        }

        public async Task<TutorRegistrationStatusDTO> GetTutorRegistrationStatusByPersonId(Guid personId)
        {


            Console.WriteLine("PersonId from TutorRepository: " + personId);
            var tutorStatus = await _databaseContext.Tutor.Include(x => x.TutorRegistrationStatus).Where(x => x.PersonId == personId).FirstOrDefaultAsync();

            Console.WriteLine("Is tutorStatus null: " + tutorStatus.GetType());

            if (tutorStatus == null)
            {
                return null;
            }
            return new TutorRegistrationStatusDTO
            {
                TutorId = tutorStatus.TutorId,
                PersonId = tutorStatus.PersonId,
                TutorRegistrationStatusId = tutorStatus.TutorRegistrationStatusId,
                TutorRegistrationStatusName = tutorStatus.TutorRegistrationStatus.Name,
                TutorRegistrationStatusDescription = tutorStatus.TutorRegistrationStatus.Description,
                IsSkippable = tutorStatus.TutorRegistrationStatus.IsSkippable,
            };
        }

        public async Task<TutorRegistrationStatusDTO> GetTutorRegistrationStatusByTutorId(Guid tutorId)
        {




            var tutorStatus = await _databaseContext.Tutor.Where(x => x.TutorId == tutorId).FirstOrDefaultAsync();


            if (tutorStatus == null)
            {
                return null;
            }
            return new TutorRegistrationStatusDTO
            {
                TutorId = tutorStatus.TutorId,
                PersonId = tutorStatus.PersonId,
                TutorRegistrationStatusId = tutorStatus.TutorRegistrationStatusId,
                TutorRegistrationStatusName = tutorStatus.TutorRegistrationStatus.Name,
                TutorRegistrationStatusDescription = tutorStatus.TutorRegistrationStatus.Description,
                IsSkippable = tutorStatus.TutorRegistrationStatus.IsSkippable,
            };
        }

        public async Task<TutorRegistrationStatusDTO> UpdateTutorRegistrationStatus(TutorRegistrationStatusDTO newStatus)
        {
            var tutorStatus = await _databaseContext.Tutor.Where(x => x.TutorId == newStatus.TutorId).FirstOrDefaultAsync();

            if (tutorStatus == null)
            {
                return null;
            }

            tutorStatus.TutorRegistrationStatusId = newStatus.TutorRegistrationStatusId;

            await _databaseContext.SaveChangesAsync();
            return newStatus;
        }
    }
}