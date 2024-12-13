using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Tutor;

namespace backend.Interfaces.Tutor
{
    public interface ITutorRepository
    {
        public Task<TutorDTO> CreateTutor(EduConnect.Entities.Tutor.Tutor tutor);
        public Task<TutorDTO> GetTutorByPersonId(Guid personId);
        public Task<TutorRegistrationStatusDTO> GetTutorRegistrationStatusByTutorId(Guid tutorId);
        public Task<TutorRegistrationStatusDTO> GetTutorRegistrationStatusByPersonId(Guid personId);
        public Task<TutorRegistrationStatusDTO> UpdateTutorRegistrationStatus(Guid personId, int tutorRegistrationStatusId);

        public Task<TutorTeachingInformationDTO> CreateTutorTeachingInformation(TutorTeachingInformationDTO tutorTeachingInformation);

        public Task<TutorTeachingInformationDTO?> GetTutorTeachingInformationByTutorId(Guid tutorId);

        public Task<TutorTeachingInformationDTO?> UpdateTutorTeachingInformation(TutorTeachingInformationDTO updateDTO);

        public Task<TutorTeachingInformationWithIncludedObjectsDTO?> GetTutorTeachingInformationWithIncludedObjectsByTutorId(Guid tutorId);

    }
}