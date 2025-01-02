using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Course.Basic;

namespace backend.Interfaces.Course
{
    public interface ICourseRepository
    {
        public Task<CourseDTO?> GetCourseByCourseName(string courseName);

        public Task<CourseDTO> CreateCourse(CourseCreateDTO createDTO);

        public Task<CourseDetailsDTO?> CreateCourseDetails(CourseDetailsCreateDTO createDTO);
    }
}