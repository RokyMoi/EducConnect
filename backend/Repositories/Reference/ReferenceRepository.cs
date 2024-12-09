using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Reference;
using backend.Interfaces.Reference;
using EduConnect.Data;
using EduConnect.Entities.Tutor;
using Microsoft.EntityFrameworkCore;
namespace backend.Repositories.Reference
{
    public class ReferenceRepository : IReferenceRepository
    {
        private readonly DataContext _dataContext;

        public ReferenceRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<List<EmploymentType>> GetAllEmploymentTypesAsync()
        {
            return await _dataContext.EmploymentType.ToListAsync();
        }

        public async Task<List<TutorRegistrationStatus>> GetAllTutorRegistrationStatusesAsync()
        {
            return await _dataContext.TutorRegistrationStatus.ToListAsync();
        }

        public async Task<List<WorkType>> GetAllWorkTypesAsync()
        {
            return await _dataContext.WorkType.ToListAsync();
        }

        public async Task<EmploymentType?> GetEmploymentTypeByIdAsync(int id)
        {
            return await _dataContext.EmploymentType.Where(e => e.EmploymentTypeId == id).FirstOrDefaultAsync();

        }

        public async Task<TutorRegistrationStatus?> GetTutorRegistrationStatusByIdAsync(int id)
        {
            return await _dataContext.TutorRegistrationStatus.Where(x => x.TutorRegistrationStatusId == id).FirstOrDefaultAsync();

        }

        public async Task<WorkType?> GetWorkTypeByIdAsync(int id)
        {
            return await _dataContext.WorkType.Where(w => w.WorkTypeId == id).FirstOrDefaultAsync();
        }
    }
}