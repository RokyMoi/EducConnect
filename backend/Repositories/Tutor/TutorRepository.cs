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

        public async Task<TutorTeachingInformationDTO> CreateTutorTeachingInformation(TutorTeachingInformationDTO tutorTeachingInformation)
        {
            //Convert the TutorTeachingInformationDTO to TutorTeachingInformation

            EduConnect.Entities.Tutor.TutorTeachingInformation tutorTeachingInformationEntity = new EduConnect.Entities.Tutor.TutorTeachingInformation
            {
                Description = tutorTeachingInformation.Description,
                TeachingStyleTypeId = tutorTeachingInformation.TeachingStyleTypeId,
                PrimaryCommunicationTypeId = tutorTeachingInformation.PrimaryCommunicationTypeId,
                SecondaryCommunicationTypeId = tutorTeachingInformation.SecondaryCommunicationTypeId,
                PrimaryEngagementMethodId = tutorTeachingInformation.PrimaryEngagementMethodId,
                SecondaryEngagementMethodId = tutorTeachingInformation.SecondaryEngagementMethodId,
                ExpectedResponseTime = tutorTeachingInformation.ExpectedResponseTime,
                SpecialConsiderations = tutorTeachingInformation.SpecialConsiderations,
                TutorId = tutorTeachingInformation.TutorId,
                CreatedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds(),


            };

            try
            {
                await _databaseContext.TutorTeachingInformation.AddAsync(tutorTeachingInformationEntity);
                await _databaseContext.SaveChangesAsync();
                return tutorTeachingInformation;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error creating tutor teaching information");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return null;
            }

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
                TutorRegistrationStatusId = tutor.TutorRegistrationStatus.TutorRegistrationStatusId,
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

        public async Task<TutorTeachingInformationDTO?> GetTutorTeachingInformationByTutorId(Guid tutorId)
        {
            var tutorTeachingInformation = await _databaseContext.TutorTeachingInformation.Where(x => x.TutorId == tutorId).FirstOrDefaultAsync();

            if (tutorTeachingInformation == null)
            {
                return null;
            }

            return new TutorTeachingInformationDTO
            {
                TutorId = tutorTeachingInformation.TutorId,
                Description = tutorTeachingInformation.Description,
                TeachingStyleTypeId = tutorTeachingInformation.TeachingStyleTypeId,
                PrimaryCommunicationTypeId = tutorTeachingInformation.PrimaryCommunicationTypeId,
                SecondaryCommunicationTypeId = tutorTeachingInformation.SecondaryCommunicationTypeId,
                PrimaryEngagementMethodId = tutorTeachingInformation.PrimaryEngagementMethodId,
                SecondaryEngagementMethodId = tutorTeachingInformation.SecondaryEngagementMethodId,
                ExpectedResponseTime = tutorTeachingInformation.ExpectedResponseTime,
                SpecialConsiderations = tutorTeachingInformation.SpecialConsiderations,
            };
        }

        public async Task<TutorTeachingInformationWithIncludedObjectsDTO?> GetTutorTeachingInformationWithIncludedObjectsByTutorId(Guid tutorId)
        {
            var tutorTeachingInformation = await _databaseContext.TutorTeachingInformation.Include(x => x.PrimaryCommunicationType).Include(x => x.SecondaryCommunicationType).Include(x => x.TeachingStyleType).Include(x => x.PrimaryEngagementMethod).Include(x => x.SecondaryEngagementMethod).Where(x => x.TutorId == tutorId).FirstOrDefaultAsync();

            if (tutorTeachingInformation == null)
            {
                return null;
            }
            return new TutorTeachingInformationWithIncludedObjectsDTO
            {
                TutorTeachingInformationId = tutorTeachingInformation.TutorTeachingInformationId,
                TutorId = tutorTeachingInformation.TutorId,
                Description = tutorTeachingInformation.Description,
                TutorTeachingStyleType = tutorTeachingInformation.TeachingStyleType,
                PrimaryCommunicationType = tutorTeachingInformation.PrimaryCommunicationType,
                SecondaryCommunicationType = tutorTeachingInformation.SecondaryCommunicationType,
                PrimaryEngagementMethod = tutorTeachingInformation.PrimaryEngagementMethod,
                SecondaryEngagementMethod = tutorTeachingInformation.SecondaryEngagementMethod,
                ExpectedResponseTime = tutorTeachingInformation.ExpectedResponseTime,
                SpecialConsiderations = tutorTeachingInformation.SpecialConsiderations,

            };
        }

        public async Task<TutorRegistrationStatusDTO> UpdateTutorRegistrationStatus(Guid personId, int newStatusId)
        {
            var tutorStatus = await _databaseContext.Tutor.Where(x => x.PersonId == personId).FirstOrDefaultAsync();

            var TutorRegistrationStatusWithId = await _databaseContext.TutorRegistrationStatus.Where(x => x.TutorRegistrationStatusId == newStatusId).FirstOrDefaultAsync();

            if (tutorStatus == null || TutorRegistrationStatusWithId == null)
            {
                return null;
            }

            tutorStatus.TutorRegistrationStatusId = newStatusId;

            await _databaseContext.SaveChangesAsync();
            return new TutorRegistrationStatusDTO
            {
                TutorId = tutorStatus.TutorId,
                PersonId = tutorStatus.PersonId,
                TutorRegistrationStatusId = tutorStatus.TutorRegistrationStatusId,
                TutorRegistrationStatusName = TutorRegistrationStatusWithId.Name,
                TutorRegistrationStatusDescription = TutorRegistrationStatusWithId.Description,
                IsSkippable = TutorRegistrationStatusWithId.IsSkippable
            };
        }

        public async Task<TutorTeachingInformationDTO?> UpdateTutorTeachingInformation(TutorTeachingInformationDTO updateDTO)
        {
            var tutorTeachingInformation = await _databaseContext.TutorTeachingInformation.Where(x => x.TutorId == updateDTO.TutorId).FirstOrDefaultAsync();

            if (tutorTeachingInformation == null)
            {
                return null;
            }

            tutorTeachingInformation.Description = updateDTO.Description;
            tutorTeachingInformation.TeachingStyleTypeId = updateDTO.TeachingStyleTypeId;
            tutorTeachingInformation.PrimaryCommunicationTypeId = updateDTO.PrimaryCommunicationTypeId;
            tutorTeachingInformation.SecondaryCommunicationTypeId = updateDTO.SecondaryCommunicationTypeId;
            tutorTeachingInformation.PrimaryEngagementMethodId = updateDTO.PrimaryEngagementMethodId;
            tutorTeachingInformation.SecondaryEngagementMethodId = updateDTO.SecondaryEngagementMethodId;
            tutorTeachingInformation.ExpectedResponseTime = updateDTO.ExpectedResponseTime;
            tutorTeachingInformation.SpecialConsiderations = updateDTO.SpecialConsiderations;

            try
            {
                await _databaseContext.SaveChangesAsync();
                return new TutorTeachingInformationDTO
                {
                    TutorId = tutorTeachingInformation.TutorId,
                    Description = tutorTeachingInformation.Description,
                    TeachingStyleTypeId = tutorTeachingInformation.TeachingStyleTypeId,
                    PrimaryCommunicationTypeId = tutorTeachingInformation.PrimaryCommunicationTypeId,
                    SecondaryCommunicationTypeId = tutorTeachingInformation.SecondaryCommunicationTypeId,
                    PrimaryEngagementMethodId = tutorTeachingInformation.PrimaryEngagementMethodId,
                    SecondaryEngagementMethodId = tutorTeachingInformation.SecondaryEngagementMethodId,
                    ExpectedResponseTime = tutorTeachingInformation.ExpectedResponseTime,
                    SpecialConsiderations = tutorTeachingInformation.SpecialConsiderations,
                };
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error updating tutor teaching information");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return null;

            }


        }
    }
}