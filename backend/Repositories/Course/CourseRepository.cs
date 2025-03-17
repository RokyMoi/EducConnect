using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Data;
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
    }
}