using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Learning;
using backend.Entities.Reference;
using backend.Interfaces.Reference;
using EduConnect.Data;
using EduConnect.Entities.Reference;
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

        public async Task AddIndustryClassificationsToDatabase(List<IndustryClassification> industryClassifications)
        {
            try
            {
                var isDatabaseEmpty = await _dataContext.IndustryClassification.FirstOrDefaultAsync();
                if (isDatabaseEmpty != null)
                {
                    return;
                }

                await _dataContext.IndustryClassification.AddRangeAsync(industryClassifications);
                await _dataContext.SaveChangesAsync();
                Console.WriteLine($"Successfully added {industryClassifications.Count} industry classifications to the database.");
            }
            catch (System.Exception ex)
            {

                Console.WriteLine($"An error occurred while adding industry classifications: {ex.Message}");

            }
        }

        public async Task AddLearningCategoriesToDatabase(List<LearningCategory> learningCategories)
        {
            try
            {
                await _dataContext.AddRangeAsync(
                    learningCategories
                );
                await _dataContext.SaveChangesAsync();
                Console.WriteLine($"Successfully added {learningCategories.Count} learning categories to the database.");

            }
            catch (System.Exception ex)
            {

                Console.WriteLine($"An error occurred while adding learning categories: {ex.Message}");
            }
        }

        public async Task AddLearningSubcategoriesToDatabase(List<LearningSubcategory> learningSubcategories)
        {


            try
            {
                await _dataContext.AddRangeAsync(learningSubcategories);
                await _dataContext.SaveChangesAsync();
                Console.WriteLine($"Successfully added {learningSubcategories.Count} learning subcategories to the database.");
            }
            catch (System.Exception ex)
            {

                Console.WriteLine($"An error occurred while adding learning subcategories: {ex.Message}");
            }
        }

        public async Task<List<CommunicationType>> GetAllCommunicationTypesAsync()
        {
            return await _dataContext.CommunicationType.ToListAsync();
        }

        public async Task<List<EmploymentType>> GetAllEmploymentTypesAsync()
        {
            return await _dataContext.EmploymentType.ToListAsync();
        }

        public async Task<List<EngagementMethod>> GetAllEngagementMethodsAsync()
        {
            return await _dataContext.EngagementMethod.ToListAsync();
        }

        public async Task<List<IndustryClassification>> GetAllIndustryClassificationsAsync()
        {
            return await _dataContext.IndustryClassification.ToListAsync();
        }

        public async Task<List<TutorRegistrationStatus>> GetAllTutorRegistrationStatusesAsync()
        {
            return await _dataContext.TutorRegistrationStatus.ToListAsync();
        }

        public async Task<List<TutorTeachingStyleType>> GetAllTutorTeachingStyleTypesAsync()
        {
            return await _dataContext.TutorTeachingStyleType.ToListAsync();
        }

        public async Task<List<WorkType>> GetAllWorkTypesAsync()
        {
            return await _dataContext.WorkType.ToListAsync();
        }

        public async Task<CommunicationType?> GetCommunicationTypeByIdAsync(int id)
        {
            return await _dataContext.CommunicationType.Where(x => x.CommunicationTypeId == id).FirstOrDefaultAsync();
        }

        public async Task<EmploymentType?> GetEmploymentTypeByIdAsync(int id)
        {
            return await _dataContext.EmploymentType.Where(e => e.EmploymentTypeId == id).FirstOrDefaultAsync();

        }

        public async Task<EngagementMethod?> GetEngagementMethodByIdAsync(int id)
        {
            return await _dataContext.EngagementMethod.Where(x => x.EngagementMethodId == id).FirstOrDefaultAsync();
        }

        public async Task<IndustryClassification?> GetIndustryClassificationByIdAsync(Guid id)
        {
            return await _dataContext.IndustryClassification.Where(x => x.IndustryClassificationId == id).FirstOrDefaultAsync();
        }

        public async Task<TutorRegistrationStatus?> GetTutorRegistrationStatusByIdAsync(int id)
        {
            return await _dataContext.TutorRegistrationStatus.Where(x => x.TutorRegistrationStatusId == id).FirstOrDefaultAsync();

        }

        public async Task<TutorTeachingStyleType?> GetTutorTeachingStyleTypeByIdAsync(int id)
        {
            return await _dataContext.TutorTeachingStyleType.Where(x => x.TutorTeachingStyleTypeId == id).FirstOrDefaultAsync();
        }

        public async Task<WorkType?> GetWorkTypeByIdAsync(int id)
        {
            return await _dataContext.WorkType.Where(w => w.WorkTypeId == id).FirstOrDefaultAsync();
        }
    }
}