using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Course.Basic;
using backend.DTOs.Course.Language;

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
    }
}