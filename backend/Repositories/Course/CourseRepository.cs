using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Course.Basic;
using backend.DTOs.Course.Language;
using backend.DTOs.Reference.Language;
using backend.Entities.Course;
using backend.Interfaces.Course;
using EduConnect.Data;
using EduConnect.Entities.Course;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
                IsDraft = createDTO.IsDraft,
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

        public async Task<CourseDTO?> GetCourseById(Guid courseId)
        {
            var course = await _dataContext.Course.Where(x => x.CourseId == courseId).FirstOrDefaultAsync();
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

        public async Task<CourseLanguageDTO?> CreateCourseLanguage(Guid courseId, Guid languageId)
        {
            var newCourseLanguage = new CourseLanguage
            {
                CourseId = courseId,
                LanguageId = languageId,
                CreatedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                UpdatedAt = null

            };

            try
            {
                await _dataContext.AddAsync(newCourseLanguage);
                await _dataContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Error creating course language");
                Console.WriteLine(ex);
                throw new Exception(ex.Message);

            }

            return new CourseLanguageDTO
            {
                CourseId = newCourseLanguage.CourseId,
                LanguageId = newCourseLanguage.LanguageId
            };

        }

        public async Task<CourseLanguageDTO?> GetCourseLanguageByCourseIdAndLanguageId(Guid courseId, Guid languageId)
        {
            var courseLanguage = await _dataContext.CourseLanguage.Where(x => x.LanguageId == languageId && x.CourseId == courseId).FirstOrDefaultAsync();

            if (courseLanguage == null)
            {
                return null;
            }

            return new CourseLanguageDTO
            {
                CourseId = courseLanguage.CourseId,
                LanguageId = courseLanguage.LanguageId
            };

        }

        public async Task<CourseDetailsDTO?> GetCourseDetailsByCourseIdAsync(Guid courseId)
        {
            var courseDetails = await _dataContext.CourseDetails.Where(x => x.CourseId == courseId).FirstOrDefaultAsync();

            if (courseDetails == null)
            {
                return null;
            }

            return new CourseDetailsDTO
            {
                CourseId = courseDetails.CourseId,
                CourseDescription = courseDetails.CourseDescription,
                Price = courseDetails.Price,
                LearningSubcategoryId = courseDetails.LearningSubcategoryId,
                LearningDifficultyLevelId = courseDetails.LearningDifficultyLevelId,
                CourseTypeId = courseDetails.CourseTypeId

            };
        }

        public async Task<List<LanguageDTO>?> GetSupportedLanguagesByCourseIdAsync(Guid courseId)
        {
            var courseLanguages = await _dataContext.CourseLanguage.Where(x => x.CourseId == courseId)
            .GroupJoin(
                _dataContext.Language,
                cl => cl.LanguageId,
                l => l.LanguageId,
                (cl, l) => new { cl, l }
            ).SelectMany(
                x => x.l.DefaultIfEmpty(),
                (x, cl) => new
                {
                    CourseLanguage = x.cl,
                    Language = cl
                }
            ).ToListAsync();
            if (courseLanguages == null || courseLanguages.Count == 0)
            {
                return null;
            }

            foreach (var courseLanguage in courseLanguages)
            {
                Console.WriteLine(
                    courseLanguage.Language.Name + " " + courseLanguage.CourseLanguage.CourseId
                );
            }

            var languages = courseLanguages.Select(
                x => new LanguageDTO
                {
                    LanguageId = x.Language.LanguageId,
                    Name = x.Language.Name,
                    Code = x.Language.Code,
                    IsRightToLeft = x.Language.IsRightToLeft

                }
            );

            return languages.ToList();
        }

        public async Task<bool> DeleteSupportedLanguageByCourseIdAndLanguageId(Guid courseId, Guid languageId)
        {
            var courseLanguage = await _dataContext.CourseLanguage.Where(x => x.CourseId == courseId && x.LanguageId == languageId).FirstOrDefaultAsync();

            if (courseLanguage == null)
            {
                return false;
            }

            _dataContext.CourseLanguage.Remove(courseLanguage);
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}