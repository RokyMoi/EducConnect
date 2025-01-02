using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Course.Basic;
using backend.DTOs.Course.Language;
using backend.Interfaces.Course;
using backend.Interfaces.Person;
using backend.Interfaces.Reference;
using backend.Interfaces.Tutor;
using backend.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Course
{
    [ApiController]
    [Route("course/create")]
    public class CourseCreateController(IPersonRepository _personRepository, ITutorRepository _tutorRepository, ICourseRepository _courseRepository, IReferenceRepository _referenceRepository) : ControllerBase
    {

        [HttpPost("basic")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> CreateCourse(CourseBasicSaveRequestDTO saveRequestDTO)
        {

            Console.WriteLine("HttpContext email: " + HttpContext.Items["Email"].ToString());

            //Check if the email in the context dictionary is null
            if (string.IsNullOrEmpty(HttpContext.Items["Email"].ToString()))
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "error",
                        message = "Something went wrong, please try again later.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            string email = HttpContext.Items["Email"].ToString();

            Guid personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());
            //Check if the PersonId from dictionary is null and if it is, call to the database to get the PersonId
            if (string.IsNullOrEmpty(HttpContext.Items["PersonId"].ToString()))
            {
                var personEmail = await _personRepository.GetPersonEmailByEmail(email);
                personId = personEmail.PersonId;
            }


            //Check if the PersonId is Tutor and if it is, check the TutorRegistrationStatus

            var tutor = await _tutorRepository.GetTutorRegistrationStatusByPersonId(personId);
            Console.WriteLine("Tutor Id: " + tutor.PersonId);

            if (tutor == null)
            {
                return Unauthorized(new
                {

                    success = "false",
                    message = "You must be a tutor to create a course.",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }


            //If tutor is not null, check the TutorRegistrationStatus is below 10 (Completed Registration)
            if (tutor != null && tutor.TutorRegistrationStatusId < 10)
            {
                return UnprocessableEntity(
                    new
                    {

                        success = "false",
                        message = "You must complete your registration first, to be able to create a course.",
                        data = new
                        {
                            CurrentTutorRegistrationStatus = new
                            {
                                TutorRegistrationStatusId = tutor.TutorRegistrationStatusId,

                            }
                        },
                        timestamp = DateTime.Now

                    }
                );
            }

            //Data validation


            //Check if the Price field from the saveRequestDTO is negative numeric value 
            if (saveRequestDTO.Price < 0)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Set price to 0 for a free course, and above 0 for a paid course.",
                    }
                );
            }

            //Check if the LearningSubcategoryId is not Guid.Empty
            if (saveRequestDTO.LearningSubcategoryId == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Subcategory is required.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the LearningDifficultyId is not a negative numeric value
            if (saveRequestDTO.LearningDifficultyLevelId < 1)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Difficulty level does not exist.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the CourseTypeId is not a negative numeric value
            if (saveRequestDTO.CourseTypeId < 1)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Course type does not exist.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check the given database against the one in the database

            //Check if there is a course with the same name
            var courseNameDuplicate = await _courseRepository.GetCourseByCourseName(saveRequestDTO.CourseName);

            if (courseNameDuplicate != null)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Course with the same name already exists.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the LearningSubcategoryId exists in the database
            var learningSubcategory = await _referenceRepository.GetLearningSubcategoryByIdAsync(saveRequestDTO.LearningSubcategoryId);

            if (learningSubcategory == null)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Subcategory does not exist.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the LearningDifficultyId exists in the database

            var learningDifficulty = await _referenceRepository.GetLearningDifficultyLevelByIdAsync(saveRequestDTO.LearningDifficultyLevelId);

            if (learningDifficulty == null)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Difficulty level does not exist.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the CourseTypeId exists in the database
            var courseType = await _referenceRepository.GetCourseTypeByIdAsync(saveRequestDTO.CourseTypeId);

            if (courseType == null)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Course type does not exist.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Create Course object and save it to the database
            var newCourse = new CourseCreateDTO
            {
                TutorId = tutor.TutorId,
                CourseName = saveRequestDTO.CourseName,
                CourseSubject = saveRequestDTO.CourseSubject,
            };
            var createdCourse = await _courseRepository.CreateCourse(newCourse); ;

            if (createdCourse == null)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "false",
                        message = "Failed to create course, please try again later.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Create CourseDetails object and save it to the database

            var newCourseDetails = new CourseDetailsCreateDTO
            {
                CourseId = createdCourse.CourseId,
                CourseDescription = saveRequestDTO.CourseDescription,
                Price = saveRequestDTO.Price,
                LearningSubcategoryId = saveRequestDTO.LearningSubcategoryId,
                LearningDifficultyLevelId = saveRequestDTO.LearningDifficultyLevelId,
                CourseTypeId = saveRequestDTO.CourseTypeId,
            };

            var createdCourseDetails = await _courseRepository.CreateCourseDetails(newCourseDetails);

            if (createdCourseDetails == null)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "false",
                        message = "Failed to create course, please try again later.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }



            return Ok(
                new
                {
                    success = "true",
                    message = "Course created successfully.",
                    data = new
                    {
                        course = createdCourse,
                        courseDetails = createdCourseDetails
                    },
                    timestamp = DateTime.Now
                }
            );
        }

        [HttpPost("language")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> AddLanguageSupportToCourse(CourseLanguageSupportSaveRequestDTO saveRequestDTO)
        {

            Console.WriteLine("HttpContext email: " + HttpContext.Items["Email"].ToString());

            //Check if the email in the context dictionary is null
            if (string.IsNullOrEmpty(HttpContext.Items["Email"].ToString()))
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "error",
                        message = "Something went wrong, please try again later.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            string email = HttpContext.Items["Email"].ToString();

            Guid personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());
            //Check if the PersonId from dictionary is null and if it is, call to the database to get the PersonId
            if (string.IsNullOrEmpty(HttpContext.Items["PersonId"].ToString()))
            {
                var personEmail = await _personRepository.GetPersonEmailByEmail(email);
                personId = personEmail.PersonId;
            }


            //Check if the PersonId is Tutor and if it is, check the TutorRegistrationStatus

            var tutor = await _tutorRepository.GetTutorRegistrationStatusByPersonId(personId);
            Console.WriteLine("Tutor Id: " + tutor.PersonId);

            if (tutor == null)
            {
                return Unauthorized(new
                {

                    success = "false",
                    message = "You must be a tutor to edit a course.",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            //Check if the courseId from the saveRequestDTO is correct Guid
            if (saveRequestDTO.CourseId == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Course identification is required",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );

            }

            //Check if the languageId from the saveRequestDTO is correct Guid
            if (saveRequestDTO.LanguageId == Guid.Empty)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "Language identification is required",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );

            }

            //Check if the course exists
            var course = await _courseRepository.GetCourseById(saveRequestDTO.CourseId);

            if (course == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Course not found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the logged in Tutor is an owner of the course (Course.TutorId equals tutor.TutorId)
            if (course.TutorId != tutor.TutorId)
            {
                return StatusCode(
                    403,
                    new
                    {
                        success = "false",
                        message = "You must be the owner of the course to edit it.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }




            //If tutor is not null, check the TutorRegistrationStatus is below 10 (Completed Registration)
            if (tutor != null && tutor.TutorRegistrationStatusId < 10)
            {
                return UnprocessableEntity(
                    new
                    {

                        success = "false",
                        message = "You must complete your registration first, to be able to create a course.",
                        data = new
                        {
                            CurrentTutorRegistrationStatus = new
                            {
                                TutorRegistrationStatusId = tutor.TutorRegistrationStatusId,

                            }
                        },
                        timestamp = DateTime.Now

                    }
                );
            }

            //Check if the languageId from the saveRequestDTO exists in the database table Languages
            var language = await _referenceRepository.GetLanguageByIdAsync(saveRequestDTO.LanguageId);

            if (language == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Language not found",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the language is already supported by the course 
            var courseLanguage = await _courseRepository.GetCourseLanguageByCourseIdAndLanguageId(course.CourseId, saveRequestDTO.LanguageId);
            Console.WriteLine("Course Language: " + courseLanguage);
            if (courseLanguage != null)
            {
                return BadRequest(
                    new
                    {
                        success = "false",
                        message = "This language is already supported by the course.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Attempt to add the language to the course
            try
            {
                await _courseRepository.CreateCourseLanguage(course.CourseId, saveRequestDTO.LanguageId);
            }
            catch (System.Exception)
            {

                return StatusCode(
                    500,
                    new
                    {
                        success = "false",
                        message = "An error occurred while adding the language to the course.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }
            return Ok(
                new
                {
                    success = "true",
                    message = "Language added to course successfully.",
                    data = new
                    {

                    },
                    timestamp = DateTime.Now
                }
            );

        }
    }
}