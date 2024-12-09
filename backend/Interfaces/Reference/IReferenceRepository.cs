using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Reference;
using EduConnect.Entities.Tutor;

namespace backend.Interfaces.Reference
{
    public interface IReferenceRepository
    {
        Task<EmploymentType?> GetEmploymentTypeByIdAsync(int id);
        Task<List<EmploymentType>> GetAllEmploymentTypesAsync();
        Task<WorkType?> GetWorkTypeByIdAsync(int id);
        Task<List<WorkType>> GetAllWorkTypesAsync();

        Task<List<TutorRegistrationStatus>> GetAllTutorRegistrationStatusesAsync();

        Task<TutorRegistrationStatus?> GetTutorRegistrationStatusByIdAsync(int id);
    }
}