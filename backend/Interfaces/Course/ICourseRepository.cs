using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Course.Basic;
using backend.DTOs.Course.CourseMainMaterial;
using backend.DTOs.Course.Language;
using backend.DTOs.Reference.Language;
using backend.Entities.Course;
using EduConnect.DTOs.Course.CourseLesson;

namespace backend.Interfaces.Course
{
    public interface ICourseRepository
    {
        public Task<CourseDTO?> GetCourseByCourseName(string courseName);

        public Task<CourseDTO> CreateCourse(CourseCreateDTO createDTO);

        public Task<CourseDetailsWithCourseTypeDTO?> CreateCourseDetails(CourseDetailsCreateDTO createDTO);

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

        public Task<List<CourseMainMaterialWithNoFileDTO>?> GetCourseMainMaterialsWithNoFilesByCourseId(Guid courseId);
        public Task<CourseMainMaterialDTO?> GetCourseMainMaterialById(Guid courseMainMaterialId);
        public Task<bool> DeleteCourseMainMaterialByCourseMainMaterialId(Guid courseMainMaterialId);

        public Task<CourseAndCourseTypeDTO?> GetCourseAndCourseTypeByCourseId(Guid courseId);

        public Task<CourseDetailsWithTutorIdDTO?>
        GetCourseDetailsWithTutorIdByCourseId(Guid courseId);

        public Task<CourseDetailsWithTutorIdDTO?> UpdateCourseTypeByCourseId(Guid courseId, int courseTypeId);

        public Task<bool> CheckIfLessonTitleExistsByCourseIdAndLessonTitle(Guid courseId, string lessonTitle);

        public Task<int?> GetHighestLessonSequenceOrderByCourseId(Guid courseId);

        public Task IncrementLessonSequenceOrders(Guid courseId, int fromPosition, int highestOrder);
        public Task IncrementAllLessonSequenceOrders(Guid courseId);

        public Task<CourseLessonDTO?> CreateCourseLesson(CourseLessonDTO courseLessonDTO);

        public Task<CourseLessonWithCourseDTO?> GetCourseLessonWithCourseByCourseLessonId(Guid courseLessonId);

        public Task<CourseLessonContentDTO?> GetCourseLessonContentByCourseLessonId(Guid courseLessonId);
        public Task<CourseLessonContentDTO?> CreateCourseCourseLessonContent(CourseLessonContentCreateDTO courseLessonContentToSave);

        public Task<long> GetTotalFileSizeOfCourseLessonSupplementaryMaterialsByCourseLessonId(Guid courseLessonId);

        public Task<int> GetCountOfCourseLessonSupplementaryMaterialsByCourseLessonId(Guid courseLessonId);

        public Task<CourseLessonSupplementaryMaterialDTO?> StoreFileToCourseLessonSupplementaryMaterial(CourseLessonSupplementaryMaterialCreateDTO courseLessonSupplementaryMaterial);
    }
}