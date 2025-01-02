using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Course.Basic;
using backend.Interfaces.Course;
using EduConnect.Data;
using EduConnect.Entities.Course;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Course
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DataContext _dataContext;

        public CourseRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<CourseDTO> CreateCourse(CourseCreateDTO createDTO)
        {
            var newCourse = new EduConnect.Entities.Course.Course
            {
                CourseId = Guid.NewGuid(),
                TutorId = createDTO.TutorId,
                CourseName = createDTO.CourseName,
                CourseSubject = createDTO.CourseSubject,
                CreatedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                UpdatedAt = null


            };

            try
            {
                await _dataContext.AddAsync(newCourse);
                await _dataContext.SaveChangesAsync();

            }
            catch (System.Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return null;
            }
            return new CourseDTO
            {
                CourseId = newCourse.CourseId,
                TutorId = newCourse.TutorId,
                CourseName = newCourse.CourseName,
                CourseSubject = newCourse.CourseSubject
            };
        }

        public async Task<CourseDetailsDTO?> CreateCourseDetails(CourseDetailsCreateDTO createDTO)
        {
            var newCourseDetails = new CourseDetails
            {
                CourseId = createDTO.CourseId,
                CourseDescription = createDTO.CourseDescription,
                Price = createDTO.Price,
                LearningSubcategoryId = createDTO.LearningSubcategoryId,
                LearningDifficultyLevelId = createDTO.LearningDifficultyLevelId,
                CourseTypeId = createDTO.CourseTypeId,
            };

            try
            {
                await _dataContext.AddAsync(newCourseDetails);
                await _dataContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error creating course details");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);

                //Remove the course if the course details creation failed
                var course = await _dataContext.Course.FirstOrDefaultAsync(c => c.CourseId == createDTO.CourseId);
                _dataContext.Course.Remove(course);
                await _dataContext.SaveChangesAsync();
                return null;
            }

            return new CourseDetailsDTO
            {
                CourseId = newCourseDetails.CourseId,
                CourseDescription = newCourseDetails.CourseDescription,
                Price = newCourseDetails.Price,
                LearningSubcategoryId = newCourseDetails.LearningSubcategoryId,
                LearningDifficultyLevelId = newCourseDetails.LearningDifficultyLevelId,
                CourseTypeId = newCourseDetails.CourseTypeId
            };
        }

        public async Task<CourseDTO?> GetCourseByCourseName(string courseName)
        {
            var course = await _dataContext.Course.Where(x => x.CourseName.Equals(courseName)).FirstOrDefaultAsync();

            if (course == null)
            {
                return null;
            }

            return new CourseDTO
            {
                CourseId = course.CourseId,
                TutorId = course.TutorId,
                CourseName = course.CourseName,
                CourseSubject = course.CourseSubject
            };
        }
    }
}