using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.Entities.Course;
using EduConnect.Interfaces.Course;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Repositories.Course
{
    public class CourseRepository(DataContext _dataContext) : ICourseRepository
    {
        public async Task<bool> CourseCategoryExistsById(Guid courseCategoryId)
        {
            return await _dataContext.CourseCategory.Where(x => x.CourseCategoryId == courseCategoryId).AnyAsync();
        }

        public async Task<bool> CourseExistsByTitle(string courseTitle)
        {
            return await _dataContext.Course.Where(x => x.Title.ToLower().Equals(courseTitle.ToLower())).AnyAsync();
        }

        public async Task<bool> CourseExistsByTitleExceptTheGivenCourseById(Guid courseId, string title)
        {
            return await _dataContext.Course
            .Where(
                x => x.CourseId != courseId && x.Title.ToLower().Equals(title.ToLower())
            )
            .AnyAsync();
        }

        public async Task<bool> CourseThumbnailExists(Guid courseId)
        {
            return await _dataContext.CourseThumbnail.Where(x => x.CourseId == courseId).AnyAsync();
        }

        public async Task<bool> CreateCourse(Entities.Course.Course course)
        {
            try
            {
                await _dataContext.Course.AddAsync(course);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to create course " + course.Title);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> CreateCourseThumbnail(CourseThumbnail courseThumbnail)
        {
            try
            {
                await _dataContext.CourseThumbnail.AddAsync(courseThumbnail);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to create course thumbnail " + courseThumbnail.CourseId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DeleteCourseThumbnail(Guid courseId)
        {
            try
            {
                var courseThumbnail = await _dataContext.CourseThumbnail.Where(x => x.CourseId == courseId).FirstOrDefaultAsync();
                _dataContext.CourseThumbnail.Remove(courseThumbnail);
                _dataContext.SaveChanges();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to delete course thumbnail " + courseId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<Entities.Course.Course>> GetAllCoursesByTutorId(Guid tutorId)
        {
            return await _dataContext.Course
            .Include(x => x.CourseCategory)
            .Include(x => x.LearningDifficultyLevel)
            .Where(x => x.TutorId == tutorId).ToListAsync();
        }

        public async Task<Entities.Course.Course?> GetCourseById(Guid courseId)
        {
            return await _dataContext.Course
            .Include(x => x.CourseCategory)
            .Include(x => x.LearningDifficultyLevel)
            .FirstOrDefaultAsync(x => x.CourseId == courseId);
        }

        public async Task<CourseThumbnail?> GetCourseThumbnailByCourseId(Guid courseId)
        {
            return await _dataContext.CourseThumbnail.Include(x => x.Course).Where(x => x.CourseId == courseId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateCourseBasics(Entities.Course.Course course)
        {
            try
            {
                _dataContext.Course.Update(course);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to update course " + course.Title);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> UpdateCourseThumbnail(CourseThumbnail courseThumbnail)
        {
            try
            {
                _dataContext.CourseThumbnail.Update(courseThumbnail);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to update course thumbnail " + courseThumbnail.CourseId);
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}