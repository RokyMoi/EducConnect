using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Course.Basic;
using backend.DTOs.Course.CourseLesson;
using backend.DTOs.Course.CourseMainMaterial;
using backend.DTOs.Course.Language;
using backend.DTOs.Reference.Language;
using backend.Entities.Course;
using backend.Interfaces.Course;
using EduConnect.Data;
using EduConnect.DTOs.Course.CourseLesson;
using EduConnect.Entities.Course;
using Microsoft.AspNetCore.Components.Web;
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
                CourseCreationCompletenessStepId = await _dataContext.CourseCreationCompletenessStep.Where(x => x.StepOrder == 0).Select(x => x.CourseCreationCompletenessStepId).FirstOrDefaultAsync(),
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

        public async Task<CourseDetailsWithCourseTypeDTO?> CreateCourseDetails(CourseDetailsCreateDTO createDTO)
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

            //Fetch CourseType
            var courseType = await _dataContext.CourseType.FirstOrDefaultAsync(ct => ct.CourseTypeId == createDTO.CourseTypeId);

            if (courseType == null)
            {
                return null;
            }
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



            return new CourseDetailsWithCourseTypeDTO
            {
                CourseId = newCourseDetails.CourseId,
                CourseDescription = newCourseDetails.CourseDescription,
                Price = newCourseDetails.Price,
                LearningSubcategoryId = newCourseDetails.LearningSubcategoryId,
                LearningDifficultyLevelId = newCourseDetails.LearningDifficultyLevelId,
                CourseType = courseType
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

        public async Task<CourseDTO?> UpdateCourseCompletenessStepByCourseIdAndStepOrder(Guid courseId, int stepOrder)
        {
            var course = await _dataContext.Course.Where(x => x.CourseId == courseId).FirstOrDefaultAsync();
            if (course == null)
            {
                return null;
            }

            var CourseCreationCompletenessStep = await _dataContext.CourseCreationCompletenessStep.Where(x => x.StepOrder == stepOrder).FirstOrDefaultAsync();
            if (CourseCreationCompletenessStep == null)
            {
                return null;
            }

            course.CourseCreationCompletenessStepId = CourseCreationCompletenessStep.CourseCreationCompletenessStepId;
            await _dataContext.SaveChangesAsync();
            return new CourseDTO
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                CourseSubject = course.CourseSubject,
                CourseCreationCompletenessStepId = course.CourseCreationCompletenessStepId
            };

        }

        public async Task<CourseMainMaterialDTO?> StoreFileToCourseMainMaterial(CourseMainMaterialDTO courseMainMaterialDTO)
        {
            var courseMainMaterial = new CourseMainMaterial
            {
                CourseMainMaterialId = Guid.NewGuid(),
                CourseId = courseMainMaterialDTO.CourseId,
                FileName = courseMainMaterialDTO.FileName,
                ContentType = courseMainMaterialDTO.ContentType,
                ContentSize = courseMainMaterialDTO.ContentSize,
                Data = courseMainMaterialDTO.Data,
                DateTimePointOfFileCreation = courseMainMaterialDTO.DateTimePointOfFileCreation,

            };
            await _dataContext.CourseMainMaterial.AddAsync(
                courseMainMaterial
            );
            await _dataContext.SaveChangesAsync();

            courseMainMaterialDTO.CourseMainMaterialId = courseMainMaterial.CourseMainMaterialId;
            return courseMainMaterialDTO;
        }

        public async Task<long> GetTotalFileSizeOfCourseMainMaterialByCourseId(Guid courseId)
        {
            return await _dataContext.CourseMainMaterial
            .Where(x => x.CourseId == courseId)
            .SumAsync(x => x.ContentSize);
        }

        public async Task<int> GetCountOfCourseMainMaterialByCourseId(Guid courseId)
        {
            return await _dataContext.CourseMainMaterial.CountAsync(
                x => x.CourseId == courseId
            );
        }

        public async Task<List<CourseMainMaterialDTO>?> GetCourseMainMaterialsByCourseId(Guid courseId)
        {
            var courseMainMaterials = await _dataContext.CourseMainMaterial.Where(x => x.CourseId == courseId).ToListAsync();

            if (courseMainMaterials.Count == 0 || courseMainMaterials == null)
            {
                return null;
            }

            return courseMainMaterials.Select(
                x => new CourseMainMaterialDTO
                {
                    CourseMainMaterialId = x.CourseMainMaterialId,
                    CourseId = x.CourseId,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                    ContentSize = x.ContentSize,
                    Data = x.Data,
                    DateTimePointOfFileCreation = x.DateTimePointOfFileCreation,
                }
            ).ToList();
        }

        public async Task<List<CourseMainMaterialWithNoFileDTO>?> GetCourseMainMaterialsWithNoFilesByCourseId(Guid courseId)
        {
            var courseMainMaterials = await _dataContext.CourseMainMaterial.Where(x => x.CourseId == courseId).Select(
                x => new CourseMainMaterialWithNoFileDTO
                {
                    CourseMainMaterialId = x.CourseMainMaterialId,
                    CourseId = x.CourseId,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                    ContentSize = x.ContentSize,
                    DateTimePointOfFileCreation = x.DateTimePointOfFileCreation,
                }
            ).ToListAsync();

            if (courseMainMaterials.Count == 0 || courseMainMaterials == null)
            {
                return null;
            }

            return courseMainMaterials;
        }

        public async Task<CourseMainMaterialDTO?> GetCourseMainMaterialById(Guid courseMainMaterialId)
        {
            return await _dataContext.CourseMainMaterial.Where(x => x.CourseMainMaterialId == courseMainMaterialId).Select(
                x => new CourseMainMaterialDTO
                {
                    CourseMainMaterialId = x.CourseMainMaterialId,
                    CourseId = x.CourseId,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                    ContentSize = x.ContentSize,
                    Data = x.Data,
                    DateTimePointOfFileCreation = x.DateTimePointOfFileCreation,
                }
            ).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteCourseMainMaterialByCourseMainMaterialId(Guid courseMainMaterialId)
        {
            var courseMainMaterial = await _dataContext.CourseMainMaterial.Where(x => x.CourseMainMaterialId == courseMainMaterialId).FirstOrDefaultAsync();
            if (courseMainMaterial == null)
            {
                return false;
            }

            _dataContext.CourseMainMaterial.Remove(courseMainMaterial);
            await _dataContext.SaveChangesAsync();
            return true;

        }

        public async Task<CourseAndCourseTypeDTO?> GetCourseAndCourseTypeByCourseId(Guid courseId)
        {
            return await _dataContext.CourseDetails
            .Include(
                x => x.CourseType
            )
            .Where(
                x => x.CourseId == courseId
            )
            .Select(
                x => new CourseAndCourseTypeDTO
                {
                    CourseId = x.CourseId,
                    CourseType = x.CourseType,
                }
            ).FirstOrDefaultAsync();
        }

        public async Task<CourseDetailsWithTutorIdDTO?> GetCourseDetailsWithTutorIdByCourseId(Guid courseId)
        {
            return await _dataContext.CourseDetails.Include(
                x => x.Course
            )
            .Where(x => x.CourseId == courseId)
            .Select(
                x => new CourseDetailsWithTutorIdDTO
                {
                    CourseId = x.CourseId,
                    CourseDescription = x.CourseDescription,
                    Price = x.Price,
                    LearningSubcategoryId = x.LearningSubcategoryId,
                    LearningDifficultyLevelId = x.LearningDifficultyLevelId,
                    CourseTypeId = x.CourseTypeId,
                    TutorId = x.Course.TutorId,

                }
            )
            .FirstOrDefaultAsync();
        }

        public async Task<CourseDetailsWithTutorIdDTO?> UpdateCourseTypeByCourseId(Guid courseId, int courseTypeId)
        {
            var courseDetails = await _dataContext.CourseDetails.Include(x => x.Course).Where(x => x.CourseId == courseId).FirstOrDefaultAsync();
            if (courseDetails == null)
            {
                return null;
            }

            courseDetails.CourseTypeId = courseTypeId;
            await _dataContext.SaveChangesAsync();
            return new CourseDetailsWithTutorIdDTO
            {
                CourseId = courseDetails.CourseId,
                CourseDescription = courseDetails.CourseDescription,
                Price = courseDetails.Price,
                LearningSubcategoryId = courseDetails.LearningSubcategoryId,
                LearningDifficultyLevelId = courseDetails.LearningDifficultyLevelId,
                CourseTypeId = courseTypeId,
                TutorId = courseDetails.Course.TutorId,
            };
        }

        public async Task<bool> CheckIfLessonTitleExistsByCourseIdAndLessonTitle(Guid courseId, string lessonTitle)
        {
            return await _dataContext.CourseLesson.Where(x => x.CourseId == courseId && x.LessonTitle == lessonTitle).AnyAsync();
        }

        public async Task<int?> GetHighestLessonSequenceOrderByCourseId(Guid courseId)
        {
            return await _dataContext.CourseLesson.Where(x => x.CourseId == courseId)
            .MaxAsync(x => (int?)x.LessonSequenceOrder);


        }

        public async Task IncrementLessonSequenceOrders(Guid courseId, int fromPosition, int highestOrder)
        {
            var lessons = await _dataContext.CourseLesson
            .Where(x => x.CourseId == courseId && x.LessonSequenceOrder >= fromPosition)
            .OrderByDescending(x => x.LessonSequenceOrder)
            .ToListAsync();

            foreach (var lesson in lessons)
            {
                lesson.LessonSequenceOrder += 1;
                lesson.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }

            await _dataContext.SaveChangesAsync();
        }

        public async Task IncrementAllLessonSequenceOrders(Guid courseId)
        {
            var lessons = await _dataContext.CourseLesson
            .Where(x => x.CourseId == courseId)
            .OrderByDescending(x => x.LessonSequenceOrder)
            .ToListAsync();

            foreach (var lesson in lessons)
            {
                lesson.LessonSequenceOrder += 1;
                lesson.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }

            await _dataContext.SaveChangesAsync();
        }

        public async Task<CourseLessonDTO?> CreateCourseLesson(CourseLessonDTO courseLessonDTO)
        {
            var newCourseLesson = new CourseLesson
            {
                CourseId = courseLessonDTO.CourseId,
                CourseLessonId = new Guid(),
                LessonTitle = courseLessonDTO.LessonTitle,
                LessonDescription = courseLessonDTO.LessonDescription,
                LessonSequenceOrder = courseLessonDTO.LessonSequenceOrder,
                LessonPrerequisites = courseLessonDTO.LessonPrerequisites,
                LessonObjective = courseLessonDTO.LessonObjective,
                LessonCompletionTimeInMinutes = courseLessonDTO.LessonCompletionTimeInMinutes,
                LessonTag = courseLessonDTO.LessonTag,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                UpdatedAt = null
            };


            try
            {
                await _dataContext.AddAsync(
                newCourseLesson
            );

                await _dataContext.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }

            return new CourseLessonDTO
            {
                CourseId = courseLessonDTO.CourseId,
                CourseLessonId = newCourseLesson.CourseLessonId,
                LessonTitle = courseLessonDTO.LessonTitle,
                LessonDescription = courseLessonDTO.LessonDescription,
                LessonSequenceOrder = courseLessonDTO.LessonSequenceOrder,
                LessonPrerequisites = courseLessonDTO.LessonPrerequisites,
                LessonObjective = courseLessonDTO.LessonObjective,
                LessonCompletionTimeInMinutes = courseLessonDTO.LessonCompletionTimeInMinutes,
                LessonTag = courseLessonDTO.LessonTag,
            };



        }

        public async Task<CourseLessonWithCourseDTO?> GetCourseLessonWithCourseByCourseLessonId(Guid courseLessonId)
        {
            var courseLesson = await _dataContext.CourseLesson
            .Include(
                x => x.Course
            )
            .Where(x => x.CourseLessonId == courseLessonId)
            .FirstOrDefaultAsync();

            if (courseLesson == null)
            {
                return null;
            }

            return new CourseLessonWithCourseDTO
            {
                CourseLessonId = courseLesson.CourseLessonId,
                LessonTitle = courseLesson.LessonTitle,
                LessonDescription = courseLesson.LessonDescription,
                LessonSequenceOrder = courseLesson.LessonSequenceOrder,
                LessonPrerequisites = courseLesson.LessonPrerequisites,
                LessonObjective = courseLesson.LessonObjective,
                LessonCompletionTimeInMinutes = courseLesson.
                LessonCompletionTimeInMinutes,
                LessonTag = courseLesson.LessonTag,
                Course = new CourseDTO
                {
                    CourseId = courseLesson.Course.CourseId,
                    TutorId = courseLesson.Course.TutorId,
                    CourseName = courseLesson.Course.CourseName,
                    CourseSubject = courseLesson.Course.CourseSubject,
                    CourseCreationCompletenessStepId = courseLesson.Course.CourseCreationCompletenessStepId,
                }
            };
        }

        public async Task<CourseLessonContentDTO?> GetCourseLessonContentByCourseLessonId(Guid courseLessonId)
        {
            var courseLessonContent = await _dataContext.CourseLessonContent.Where(x => x.CourseLessonId == courseLessonId).FirstOrDefaultAsync();

            if (courseLessonContent == null)
            {
                return null;
            }

            return new CourseLessonContentDTO
            {
                CourseLessonId = courseLessonContent.CourseLessonId,
                CourseLessonContentId = courseLessonContent.CourseLessonContentId,
                Title = courseLessonContent.Title,
                Description = courseLessonContent.Description,
                ContentData = courseLessonContent.ContentData,
            };

        }

        public async Task<CourseLessonContentDTO?> CreateCourseCourseLessonContent(CourseLessonContentCreateDTO courseLessonContentToSave)
        {
            var newCourseLessonContent = new CourseLessonContent
            {
                CourseLessonContentId = Guid.NewGuid(),
                CourseLessonId = courseLessonContentToSave.CourseLessonId,
                Title = courseLessonContentToSave.Title,
                Description = courseLessonContentToSave.Description,
                ContentData = courseLessonContentToSave.ContentData,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                UpdatedAt = null
            };

            try
            {
                await _dataContext.AddAsync(newCourseLessonContent);
                await _dataContext.SaveChangesAsync();


            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }
            return new CourseLessonContentDTO
            {
                CourseLessonContentId = newCourseLessonContent.CourseLessonContentId,
                CourseLessonId = newCourseLessonContent.CourseLessonId,
                Title = newCourseLessonContent.Title,
                Description = newCourseLessonContent.Description,
                ContentData = newCourseLessonContent.ContentData,
            };
        }

        public async Task<long> GetTotalFileSizeOfCourseLessonSupplementaryMaterialsByCourseLessonId(Guid courseLessonId)
        {
            return await _dataContext.CourseLessonSupplementaryMaterial.Where(
                x => x.CourseLessonId == courseLessonId
            ).SumAsync(x => x.ContentSize);
        }

        public async Task<int> GetCountOfCourseLessonSupplementaryMaterialsByCourseLessonId(Guid courseLessonId)
        {
            return await _dataContext.CourseLessonSupplementaryMaterial.Where(
                x => x.CourseLessonId == courseLessonId
            )
            .CountAsync();

        }

        public async Task<CourseLessonSupplementaryMaterialDTO?> StoreFileToCourseLessonSupplementaryMaterial(CourseLessonSupplementaryMaterialCreateDTO courseLessonSupplementaryMaterial)
        {
            var newCourseLessonSupplementaryMaterial = new CourseLessonSupplementaryMaterial
            {
                CourseLessonSupplementaryMaterialId = Guid.NewGuid(),
                CourseLessonId = courseLessonSupplementaryMaterial.CourseLessonId,
                FileName = courseLessonSupplementaryMaterial.FileName,
                ContentType = courseLessonSupplementaryMaterial.ContentType,
                ContentSize = courseLessonSupplementaryMaterial.ContentSize,
                Data = courseLessonSupplementaryMaterial.Data,
                DateTimePointOfFileCreation = courseLessonSupplementaryMaterial.DateTimePointOfFileCreation,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                UpdatedAt = null

            };

            await _dataContext.AddAsync(newCourseLessonSupplementaryMaterial);
            await _dataContext.SaveChangesAsync();

            return new CourseLessonSupplementaryMaterialDTO
            {
                CourseLessonSupplementaryMaterialId = newCourseLessonSupplementaryMaterial.CourseLessonSupplementaryMaterialId,
                CourseLessonId = newCourseLessonSupplementaryMaterial.CourseLessonId,
                FileName = newCourseLessonSupplementaryMaterial.FileName,
                ContentType = newCourseLessonSupplementaryMaterial.ContentType,
                ContentSize = newCourseLessonSupplementaryMaterial.ContentSize,
                Data = newCourseLessonSupplementaryMaterial.Data,
                DateTimePointOfFileCreation = newCourseLessonSupplementaryMaterial.DateTimePointOfFileCreation,
            };
        }

        public async Task<List<CourseLessonSupplementaryMaterialWithNoFileDTO>?> GetCourseLessonSupplementaryMaterialsWithNoFilesByCourseLessonId(Guid courseLessonId)
        {
            var courseLessonSupplementaryMaterials = await _dataContext.CourseLessonSupplementaryMaterial
            .Where(
                x => x.CourseLessonId == courseLessonId
            )
            .Select(
                x => new
                {
                    x.CourseLessonSupplementaryMaterialId,
                    x.CourseLessonId,
                    x.FileName,
                    x.ContentType,
                    x.ContentSize,
                    x.DateTimePointOfFileCreation,
                    x.CreatedAt,

                }
            )
            .ToListAsync();


            if (courseLessonSupplementaryMaterials == null)
            {
                return null;
            }

            return [.. courseLessonSupplementaryMaterials
            .Select(
                x => new CourseLessonSupplementaryMaterialWithNoFileDTO
                {
                    CourseLessonId = x.CourseLessonId,
                    CourseLessonSupplementaryMaterialId = x.CourseLessonSupplementaryMaterialId,
                    ContentType = x.ContentType,
                    ContentSize = x.ContentSize,
                    FileName = x.FileName,
                    DateTimePointOfFileCreation = x.DateTimePointOfFileCreation,
                    CreatedAt = x.CreatedAt,
                }
            )];

        }

        public async Task<CourseLessonSupplementaryMaterialDTO?> GetCourseLessonSupplementaryMaterialById(Guid courseLessonSupplementaryMaterialId)
        {

            var courseLessonSupplementaryMaterial = await _dataContext.CourseLessonSupplementaryMaterial.Where(
                x => x.CourseLessonSupplementaryMaterialId == courseLessonSupplementaryMaterialId
            )
            .Select(
                x => new
                {
                    x.CourseLessonSupplementaryMaterialId,
                    x.CourseLessonId,
                    x.FileName,
                    x.ContentType,
                    x.ContentSize,
                    x.Data,
                    x.DateTimePointOfFileCreation,
                }
            ).FirstOrDefaultAsync();

            if (courseLessonSupplementaryMaterial == null)
            {
                return null;
            }

            return new CourseLessonSupplementaryMaterialDTO
            {
                CourseLessonId = courseLessonSupplementaryMaterial.CourseLessonId,
                CourseLessonSupplementaryMaterialId = courseLessonSupplementaryMaterial.CourseLessonSupplementaryMaterialId,
                ContentType = courseLessonSupplementaryMaterial.ContentType,
                ContentSize = courseLessonSupplementaryMaterial.ContentSize,
                FileName = courseLessonSupplementaryMaterial.FileName,
                Data = courseLessonSupplementaryMaterial.Data,
                DateTimePointOfFileCreation = courseLessonSupplementaryMaterial.DateTimePointOfFileCreation,
            };
        }

        public async Task<CourseSupplementaryMaterialReferenceDTO?> GetCourseSupplementaryMaterialReferenceByCourseLessonSupplementaryMaterialId(Guid courseLessonSupplementaryMaterialId)
        {
            var courseLessonSupplementaryMaterial = await _dataContext.CourseLessonSupplementaryMaterial
            .Include(x => x.CourseLesson.Course)
            .Where(
                x => x.CourseLessonSupplementaryMaterialId == courseLessonSupplementaryMaterialId
            )
            .Select(
                x =>
                new
                {
                    x.CourseLessonSupplementaryMaterialId,
                    x.CourseLessonId,
                    x.CourseLesson.CourseId,
                    x.CourseLesson.Course.TutorId
                }

            )
            .FirstOrDefaultAsync();

            if (courseLessonSupplementaryMaterial == null)
            {
                return null;
            }

            return new CourseSupplementaryMaterialReferenceDTO
            {
                CourseLessonSupplementaryMaterialId = courseLessonSupplementaryMaterial.CourseLessonSupplementaryMaterialId,
                CourseLessonId = courseLessonSupplementaryMaterial.CourseLessonId,
                CourseId = courseLessonSupplementaryMaterial.CourseId,
                TutorId = courseLessonSupplementaryMaterial.TutorId,
            };
        }

        public async Task<bool> DeleteCourseLessonSupplementaryMaterialByCourseLessonSupplementaryMaterialId(Guid courseLessonSupplementaryMaterialId)
        {
            var courseLessonSupplementaryMaterial = await _dataContext.CourseLessonSupplementaryMaterial.Where(
                x => x.CourseLessonSupplementaryMaterialId == courseLessonSupplementaryMaterialId
            ).FirstOrDefaultAsync();

            if (courseLessonSupplementaryMaterial == null)
            {
                return false;
            }

            try
            {
                _dataContext.CourseLessonSupplementaryMaterial.Remove(courseLessonSupplementaryMaterial);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to delete course lesson supplementary material: " + ex.Message);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<CourseLessonShorthandDTO>?> GetCourseLessonShorthandListByCourseId(Guid courseId)
        {

            return await _dataContext.CourseLesson
            .Where(x => x.CourseId == courseId)
            .GroupJoin(
                _dataContext.CourseLessonSupplementaryMaterial,
                ccl => ccl.CourseLessonId,
                clsm => clsm.CourseLessonId,
                (ccl, clsmGroup) => new
                {
                    CourseLesson = ccl,
                    SupplementaryMaterials = clsmGroup
                }
            )
            .Select(
                x => new CourseLessonShorthandDTO
                {
                    CourseLessonId = x.CourseLesson.CourseLessonId,
                    CourseId = x.CourseLesson.CourseId,
                    LessonTitle = x.CourseLesson.LessonTitle,
                    LessonTag = x.CourseLesson.LessonTag,
                    LessonSequenceOrder = x.CourseLesson.LessonSequenceOrder,
                    CourseLessonSupplementaryMaterialCount = x.SupplementaryMaterials.Count(),
                    CourseLessonSupplementaryMaterialTotalSize = x.SupplementaryMaterials.Sum(y => (long?)y.ContentSize) ?? 0,
                    CreatedAt = x.CourseLesson.CreatedAt,
                }
            )
            .OrderBy(x => x.LessonSequenceOrder)
            .ToListAsync();



        }

        public async Task<bool> CheckIfCourseExistsByCourseId(Guid courseId)
        {
            return await _dataContext.Course.Where(x => x.CourseId == courseId).AnyAsync();
        }

        public async Task<CourseLessonReferenceDTO?> GetCourseLessonReferenceByCourseLessonId(Guid courseLessonId)
        {
            var courseLesson = await _dataContext.CourseLesson.Include(x => x.Course)
            .Where(x => x.CourseLessonId == courseLessonId)
            .Select(
                x => new CourseLessonReferenceDTO
                {
                    CourseId = x.CourseId,
                    CourseLessonId = x.CourseLessonId,
                    TutorId = x.Course.TutorId
                }
            )
            .FirstOrDefaultAsync();

            if (courseLesson == null)
            {
                return null;
            }

            return courseLesson;
        }

        public async Task<bool> DeleteCourseLessonAndAssociatedDataByCourseLessonId(Guid courseLessonId)
        {
            var lesson = await _dataContext.CourseLesson.Where(x => x.CourseLessonId == courseLessonId).FirstOrDefaultAsync();

            if (lesson == null)
            {
                Console.WriteLine("CourseLesson not found");
                return false;
            }

            var sequenceOrder = lesson.LessonSequenceOrder;
            var courseId = lesson.CourseId;

            var lessonContents = await _dataContext.CourseLessonContent.Where(x => x.CourseLessonId == courseLessonId).ToListAsync();
            _dataContext.CourseLessonContent.RemoveRange(lessonContents);

            var lessonSupplementaryMaterials = await _dataContext.CourseLessonSupplementaryMaterial.Where(x => x.CourseLessonId == courseLessonId).ToListAsync();
            _dataContext.CourseLessonSupplementaryMaterial.RemoveRange(lessonSupplementaryMaterials);

            _dataContext.CourseLesson.Remove(lesson);

            await _dataContext.SaveChangesAsync();

            await ReorderCourseLessonSequenceOrderAfterDeletion(courseId, sequenceOrder);
            return true;
        }

        public async Task ReorderCourseLessonSequenceOrderAfterDeletion(Guid courseId, int deletedLessonSequenceOrder)
        {
            var lessonsToUpdate = await _dataContext.CourseLesson
            .Where(x => x.CourseId == courseId && x.LessonSequenceOrder > deletedLessonSequenceOrder)
            .OrderBy(x => x.LessonSequenceOrder)
            .ToListAsync();




            foreach (var lesson in lessonsToUpdate)
            {
                Console.WriteLine($"Lesson sequence order before: {lesson.LessonSequenceOrder}");
                lesson.LessonSequenceOrder = deletedLessonSequenceOrder++;
                Console.WriteLine($"Lesson sequence order after: {lesson.LessonSequenceOrder}");

            }

            await _dataContext.SaveChangesAsync();
        }

        public async Task<CourseLessonWithContentAndSupplementaryMaterialsDTO?> GetCourseLessonWithContentAndSupplementaryMaterialsByCourseLessonId(Guid courseLessonId)
        {
            return await _dataContext.CourseLesson
            .Where(x => x.CourseLessonId == courseLessonId)
            .Include(x => x.Course)
            .Select(
                x => new CourseLessonWithContentAndSupplementaryMaterialsDTO
                {
                    TutorId = x.Course.TutorId,
                    CourseId = x.CourseId,
                    CourseLesson = new CourseLessonDTO
                    {
                        CourseLessonId = x.CourseLessonId,
                        CourseId = x.CourseId,
                        LessonTitle = x.LessonTitle,
                        LessonDescription = x.LessonDescription,
                        LessonSequenceOrder = x.LessonSequenceOrder,
                        LessonPrerequisites = x.LessonPrerequisites,
                        LessonObjective = x.LessonObjective,
                        LessonCompletionTimeInMinutes = x.LessonCompletionTimeInMinutes,
                        LessonTag = x.LessonTag,
                        CreatedAt = x.CreatedAt,
                    },
                    CourseLessonContent = _dataContext.CourseLessonContent.Where(y => y.CourseLessonId == x.CourseLessonId)
                    .Select(
                        y => new CourseLessonContentDTO
                        {
                            CourseLessonId = y.CourseLessonId,
                            CourseLessonContentId = y.CourseLessonContentId,
                            Title = y.Title,
                            Description = y.Description,
                            ContentData = y.ContentData,
                        }
                    )
                    .FirstOrDefault(),
                    CourseLessonSupplementaryMaterials = _dataContext.CourseLessonSupplementaryMaterial.Where(y => y.CourseLessonId == x.CourseLessonId)
                    .Select(sm =>
                        new CourseLessonSupplementaryMaterialWithNoFileDTO
                        {
                            CourseLessonId = sm.CourseLessonId,
                            CourseLessonSupplementaryMaterialId = sm.CourseLessonSupplementaryMaterialId,
                            ContentType = sm.ContentType,
                            ContentSize = sm.ContentSize,
                            FileName = sm.FileName,
                            DateTimePointOfFileCreation = sm.DateTimePointOfFileCreation,
                            CreatedAt = sm.CreatedAt,
                        }

                    ).ToList()

                }
            ).FirstOrDefaultAsync();







        }

        public async Task<CourseLessonWithCourseLessonContentDTO?> GetCourseLessonWithCourseLessonContentByCourseLessonId(Guid courseLessonId)
        {
            return await _dataContext.CourseLesson.Include(x => x.Course).Where(x => x.CourseLessonId == courseLessonId).Select(
                x => new CourseLessonWithCourseLessonContentDTO
                {
                    CourseLessonId = x.CourseLessonId,
                    CourseId = x.CourseId,
                    TutorId = x.Course.TutorId,
                    CourseLesson = new CourseLessonDTO
                    {
                        CourseLessonId = x.CourseLessonId,
                        CourseId = x.CourseId,
                        LessonTitle = x.LessonTitle,
                        LessonDescription = x.LessonDescription,
                        LessonSequenceOrder = x.LessonSequenceOrder,
                        LessonPrerequisites = x.LessonPrerequisites,
                        LessonObjective = x.LessonObjective,
                        LessonCompletionTimeInMinutes = x.LessonCompletionTimeInMinutes,
                        LessonTag = x.LessonTag,
                    },
                    CourseLessonContent = _dataContext.CourseLessonContent.Where(y => y.CourseLessonId == x.CourseLessonId).Select(
                        clc => new CourseLessonContentDTO
                        {
                            CourseLessonId = clc.CourseLessonId,
                            CourseLessonContentId = clc.CourseLessonContentId,
                            Title = clc.Title,
                            Description = clc.Description,
                            ContentData = clc.ContentData,
                        }
                    ).FirstOrDefault(),
                }
            ).FirstOrDefaultAsync();
        }

        public async Task<CourseLessonWithCourseLessonContentDTO?> UpdateCourseLessonAndCourseLessonContentByCourseLessonId(Guid courseLessonId, CourseLessonDTO courseLesson, CourseLessonContentDTO courseLessonContent)
        {
            var courseLessonToUpdate = await _dataContext.CourseLesson.Include(x => x.Course).Where(x => x.CourseLessonId == courseLessonId).FirstOrDefaultAsync();

            if (courseLessonToUpdate == null)
            {
                return null;
            }

            var courseLessonContentToUpdate = await _dataContext.CourseLessonContent.Where(x => x.CourseLessonId == courseLessonId).FirstOrDefaultAsync();

            if (courseLessonContentToUpdate == null)
            {
                return null;
            }

            //Logic for reordering lessons sequence order if the lesson sequence order is changed
            // Store the current lesson sequence order
            int currentLessonSequenceOrder = courseLessonToUpdate.LessonSequenceOrder;
            // Get the total number of lessons for the course
            int totalLessons = await _dataContext.CourseLesson
                .CountAsync(l => l.CourseId == courseLessonToUpdate.CourseId);

            // Ensure the new position does not exceed the total number of lessons
            int newLessonSequenceOrder = Math.Min(courseLesson.LessonSequenceOrder, totalLessons);

            Console.WriteLine($"Current Lesson Sequence Order: {currentLessonSequenceOrder}");
            Console.WriteLine($"New Lesson Sequence Order: {newLessonSequenceOrder}");
            Console.WriteLine($"Total Lessons: {totalLessons}");



            if (currentLessonSequenceOrder != newLessonSequenceOrder)
            {
                if (currentLessonSequenceOrder < newLessonSequenceOrder)
                {
                    // Moving lesson down: Shift other lessons up (decrement order)
                    var lessonsToShiftUp = await _dataContext.CourseLesson
                        .Where(l => l.CourseId == courseLessonToUpdate.CourseId &&
                                    l.LessonSequenceOrder > currentLessonSequenceOrder &&
                                    l.LessonSequenceOrder <= newLessonSequenceOrder)
                        .ToListAsync();

                    foreach (var lesson in lessonsToShiftUp)
                    {
                        lesson.LessonSequenceOrder--;
                    }
                }
                else
                {
                    // Moving lesson up: Shift other lessons down (increment order)
                    var lessonsToShiftDown = await _dataContext.CourseLesson
                        .Where(l => l.CourseId == courseLessonToUpdate.CourseId &&
                                    l.LessonSequenceOrder < currentLessonSequenceOrder &&
                                    l.LessonSequenceOrder >= newLessonSequenceOrder)
                        .ToListAsync();

                    foreach (var lesson in lessonsToShiftDown)
                    {
                        lesson.LessonSequenceOrder++;
                    }
                }

                // Assign the new sequence order to the updated lesson
                courseLessonToUpdate.LessonSequenceOrder = newLessonSequenceOrder;
            }


            courseLessonToUpdate.LessonTitle = courseLesson.LessonTitle;
            courseLessonToUpdate.LessonDescription = courseLesson.LessonDescription;
            courseLessonToUpdate.LessonPrerequisites = courseLesson.LessonPrerequisites;
            courseLessonToUpdate.LessonObjective = courseLesson.LessonObjective;
            courseLessonToUpdate.LessonCompletionTimeInMinutes = courseLesson.LessonCompletionTimeInMinutes;
            courseLessonToUpdate.LessonTag = courseLesson.LessonTag;
            courseLessonToUpdate.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            courseLessonContentToUpdate.Title = courseLessonContent.Title;
            courseLessonContentToUpdate.Description = courseLessonContent.Description;
            courseLessonContentToUpdate.ContentData = courseLessonContent.ContentData;
            courseLessonContentToUpdate.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await _dataContext.SaveChangesAsync();

            return new CourseLessonWithCourseLessonContentDTO
            {
                CourseLessonId = courseLessonId,
                CourseId = courseLessonToUpdate.CourseId,
                TutorId = courseLessonToUpdate.Course.TutorId,
                CourseLesson = courseLesson,
                CourseLessonContent = courseLessonContent,
            };

        }

        public async Task UpdateLessonOrderAsync(int oldPosition, int newPosition)
        {

            // Get the lesson that needs to be moved
            var lessonToMove = await _dataContext.CourseLesson
                .FirstOrDefaultAsync(l => l.LessonSequenceOrder == oldPosition);

            if (lessonToMove == null)
            {
                throw new InvalidOperationException("Lesson not found.");
            }

            if (oldPosition < newPosition)
            {
                // Shift lessons up (decrease order by 1)
                var lessonsToShiftUp = await _dataContext.CourseLesson
                    .Where(l => l.LessonSequenceOrder > oldPosition && l.LessonSequenceOrder <= newPosition)
                    .ToListAsync();

                foreach (var lesson in lessonsToShiftUp)
                {
                    lesson.LessonSequenceOrder--;
                }
            }
            else
            {
                // Shift lessons down (increase order by 1)
                var lessonsToShiftDown = await _dataContext.CourseLesson
                    .Where(l => l.LessonSequenceOrder < oldPosition && l.LessonSequenceOrder >= newPosition)
                    .ToListAsync();

                foreach (var lesson in lessonsToShiftDown)
                {
                    lesson.LessonSequenceOrder++;
                }
            }

            // Set the new position for the moved lesson
            lessonToMove.LessonSequenceOrder = newPosition;

            // Save changes to the database
            await _dataContext.SaveChangesAsync();
        }

    }

}