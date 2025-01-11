using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Course.Basic;
using backend.DTOs.Course.CourseMainMaterial;
using backend.DTOs.Course.Language;
using backend.DTOs.Reference.Language;

namespace backend.Interfaces.Course
{
    public interface ICourseRepository
    {
        public Task<CourseDTO?> GetCourseByCourseName(string courseName);

        public Task<CourseDTO> CreateCourse(CourseCreateDTO createDTO);

        public Task<CourseDetailsDTO?> CreateCourseDetails(CourseDetailsCreateDTO createDTO);

        public Task<CourseDTO?> GetCourseById(Guid courseId);

        public Task<CourseLanguageDTO?> GetCourseLanguageByCourseIdAndLanguageId(Guid courseId, Guid languageId);

        public Task<CourseLanguageDTO?> CreateCourseLanguage(Guid courseId, Guid languageId);

        public Task<CourseDetailsDTO?> GetCourseDetailsByCourseIdAsync(Guid courseId);

        public Task<List<LanguageDTO>?> GetSupportedLanguagesByCourseIdAsync(Guid courseId);

        public Task<bool> DeleteSupportedLanguageByCourseIdAndLanguageId(Guid courseId, Guid languageId);

        public Task<CourseDTO?> UpdateCourseCompletenessStepByCourseIdAndStepOrder(Guid courseId, int stepOrder);

        public Task<CourseMainMaterialDTO?> StoreFileToCourseMainMaterial(CourseMainMaterialDTO courseMainMaterialDTO);
        public Task<long> GetTotalFileSizeOfCourseMainMaterialByCourseId(Guid courseId);

        public Task<int> GetCountOfCourseMainMaterialByCourseId(Guid courseId);

        public Task<List<CourseMainMaterialDTO>?>
        GetCourseMainMaterialsByCourseId(Guid courseId);
    }
}