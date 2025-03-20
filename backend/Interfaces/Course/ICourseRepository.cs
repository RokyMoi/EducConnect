using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Interfaces.Course
{
    public interface ICourseRepository
    {
        Task<bool> CourseExistsByTitle(string courseTitle);
        Task<bool> CourseCategoryExistsById(Guid courseCategoryId);

        Task<bool> CreateCourse(Entities.Course.Course course);

        Task<List<Entities.Course.Course>> GetAllCoursesByTutorId(Guid tutorId);

        Task<Entities.Course.Course?> GetCourseById(Guid courseId);

        Task<bool> UpdateCourseBasics(Entities.Course.Course course);

        Task<bool> CourseExistsByTitleExceptTheGivenCourseById(Guid courseId, string title);

        Task<bool> CreateCourseThumbnail(Entities.Course.CourseThumbnail courseThumbnail);

        Task<bool> CourseThumbnailExists(Guid courseId);

        Task<Entities.Course.CourseThumbnail?> GetCourseThumbnailByCourseId(Guid courseId);

        Task<bool> UpdateCourseThumbnail(Entities.Course.CourseThumbnail courseThumbnail);

        Task<bool> DeleteCourseThumbnail(Guid courseId);
    }
}