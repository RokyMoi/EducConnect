using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Learning;
using backend.DTOs.Reference.LearningDifficultyLevel;
using backend.DTOs.Reference.LearningSubcategory;
using backend.Entities.Course;
using backend.Entities.Learning;
using backend.Entities.Reference;
using backend.Entities.Reference.Language;
using backend.Entities.Reference.Learning;
using EduConnect.Entities.Reference;
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

        Task<TutorTeachingStyleType?> GetTutorTeachingStyleTypeByIdAsync(int id);
        Task<List<TutorTeachingStyleType>> GetAllTutorTeachingStyleTypesAsync();
        Task<CommunicationType?> GetCommunicationTypeByIdAsync(int id);

        Task<List<CommunicationType>> GetAllCommunicationTypesAsync();
        Task<EngagementMethod?> GetEngagementMethodByIdAsync(int id);
        Task<List<EngagementMethod>> GetAllEngagementMethodsAsync();

        public Task AddIndustryClassificationsToDatabase(List<IndustryClassification> industryClassifications);

        public Task<List<IndustryClassification>> GetAllIndustryClassificationsAsync();

        public Task<IndustryClassification?> GetIndustryClassificationByIdAsync(Guid id);

        public Task AddLearningCategoriesToDatabase(
            List<LearningCategory> learningCategories
        );

        public Task AddLearningSubcategoriesToDatabase(
            List<EduConnect.Entities.Learning.LearningSubcategory> learningSubcategories
        );

        public Task<LearningDifficultyLevelDTO?> GetLearningDifficultyLevelByIdAsync(
            int id
        );

        public Task<LearningSubcategoryDTO?> GetLearningSubcategoryByIdAsync(
            Guid id
        );

        public Task<CourseType?> GetCourseTypeByIdAsync(int id);

        public Task<Language?> GetLanguageByIdAsync(Guid id);

        public Task<LearningCategoryListAndLearningSubcategoryListDTO?>
        GetAllLearningCategoriesAndSubcategories();

        public Task<List<LearningDifficultyLevel>> GetAllLearningDifficultyLevelsAsync();

        public Task<List<CourseType>> GetAllCourseTypesAsync();
    }
}