using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Course.Language;
using backend.Interfaces.Course;
using backend.Interfaces.Person;
using backend.Interfaces.Reference;
using backend.Interfaces.Tutor;
using backend.Middleware;
using EduConnect.Migrations;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Course
{
    [ApiController]
    [Route("course")]
    public class CourseController(IPersonRepository _personRepository, ITutorRepository _tutorRepository, ICourseRepository _courseRepository, IReferenceRepository _referenceRepository) : ControllerBase
    {
        [HttpGet("basic/{courseId}")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> GetCourseById(Guid courseId)
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

            //Check if the course exists
            var course = await _courseRepository.GetCourseById(courseId);
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

            //Get the CourseDetails with the CourseId 
            var courseDetails = await _courseRepository.GetCourseDetailsByCourseIdAsync(courseId);
            if (courseDetails == null)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "false",
                        message = "Something went wrong, please try again later.",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }



            return Ok(

                new
                {
                    success = "true",
                    message = "Course retrieved successfully",
                    data = new
                    {
                        course = course,
                        courseDetails = courseDetails
                    },
                    timestamp = DateTime.Now
                }
            );
        }

        [HttpGet("supported-language/{courseId}")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> GetSupportedLanguagesByCourseId([FromRoute] Guid courseId)
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

            Console.WriteLine("Check course:" + courseId);
            //Check if the course exists
            var course = await _courseRepository.GetCourseById(courseId);
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

            //Get the list of supported languages (all records from the CourseLanguage table with the given CourseId joined with the Language table using LanguageId)
            var supportedLanguages = await _courseRepository.GetSupportedLanguagesByCourseIdAsync(courseId);

            if (supportedLanguages == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "No supported languages found for the selected course",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }


            return Ok(
                new
                {
                    success = "true",
                    message = $"Found {supportedLanguages.Count} supported languages for the selected course",
                    data = new
                    {

                        supportedLanguages = supportedLanguages,
                    },
                    timestamp = DateTime.Now

                }
            );

        }

        [HttpDelete("supported-language/remove")]
        [CheckPersonLoginSignup]
        public async Task<IActionResult> DeleteSupportedLanguage(DeleteSupportedLanguageRequestDTO requestDTO)
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

            //Check if the course exists
            var course = await _courseRepository.GetCourseById(requestDTO.CourseId);
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

            //Check if the language exists
            var language = await _referenceRepository.GetLanguageByIdAsync(requestDTO.LanguageId);
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

            //Check if the course has the language set as supported
            var supportedLanguage = await _courseRepository.GetCourseLanguageByCourseIdAndLanguageId(requestDTO.CourseId, requestDTO.LanguageId);
            if (supportedLanguage == null)
            {
                return NotFound(
                    new
                    {
                        success = "false",
                        message = "Language is not supported by the course",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Check if the tutor is the owner of the course
            if (course.TutorId != tutor.TutorId)
            {
                return StatusCode(
                    403,
                    new
                    {
                        success = "false",
                        message = "You are not the owner of this course",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            //Delete the supported language
            var deleteResult = await _courseRepository.DeleteSupportedLanguageByCourseIdAndLanguageId(requestDTO.CourseId, requestDTO.LanguageId);

            if (!deleteResult)
            {
                return StatusCode(
                    500,
                    new
                    {
                        success = "false",
                        message = "Failed to delete the supported language",
                        data = new { },
                        timestamp = DateTime.Now
                    }
                );
            }

            return Ok(
                new
                {
                    success = "true",
                    message = "Supported language deleted successfully",
                    data = new
                    {
                        supportedLanguage = supportedLanguage.LanguageId,
                    },
                    timestamp = DateTime.Now
                }
            );
        }
    }
}