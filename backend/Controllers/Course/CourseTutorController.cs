using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces.Reference;
using backend.Interfaces.Tutor;
using EduConnect.DTOs.Course;
using EduConnect.Entities;
using EduConnect.Interfaces.Course;
using EduConnect.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Controllers.Course
{
    [ApiController]
    [Route("tutor/course")]
    [AuthenticationGuard(isTutor: true, isAdmin: false, isStudent: false)]
    public class CourseTutorController(
        ICourseRepository _courseRepository,
        IReferenceRepository _referenceRepository,
        ITutorRepository _tutorRepository
    ) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
        {
            //Check if any of the courses contain the same title
            var courseTitleTaken = await _courseRepository.CourseExistsByTitle(request.Title);

            if (courseTitleTaken)
            {
                return Conflict(
                    ApiResponse<object>.GetApiResponse(
                       "You  cannot create a course with the same title as an existing course",
                       null
                    )
                );
            }

            //Check if the CourseCategoryId exists
            var courseCategoryExists = await _courseRepository.CourseCategoryExistsById(request.CourseCategoryId);

            if (!courseCategoryExists)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "The course category does not exist",
                        null
                    )
                );

            }

            //Check if the LearningDifficultyLevelId exists
            var learningDifficultyLevelExists = await _referenceRepository.LearningDifficultyLevelExistsById(request.LearningDifficultyLevelId);

            if (!learningDifficultyLevelExists)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "The learning difficulty level not found",
                        null
                    )
                );
            }


            //Check if the minimum number of students and maximum number of students (if provided respectively) are greater than or equal to zero, and the minimum number of students is less than maximum number of students
            if (request.MinNumberOfStudents.HasValue && request.MaxNumberOfStudents.HasValue && request.MinNumberOfStudents >= request.MaxNumberOfStudents)
            {
                return BadRequest(
                    ApiResponse<object>.GetApiResponse(
                        "The minimum number of students must be less than the maximum number of students",
                        null
                    )
                );

            }

            //Get the TutorId from the PersonId
            var personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());


            Console.WriteLine("Get Tutor by this PersonId: " + personId);
            var tutor = await _tutorRepository.GetTutorByPersonId(personId);

            Console.WriteLine("Is tutor null? " + (tutor == null));

            var course = new Entities.Course.Course
            {
                CourseId = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                CourseCategoryId = request.CourseCategoryId,
                TutorId = tutor.TutorId,
                LearningDifficultyLevelId = request.LearningDifficultyLevelId,
                MinNumberOfStudents = request.MinNumberOfStudents,
                MaxNumberOfStudents = request.MaxNumberOfStudents,
                Price = request.Price,
                PublishedStatus = false,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdatedAt = null

            };

            Console.WriteLine(course);

            var courseCreateResult = await _courseRepository.CreateCourse(course);

            if (!courseCreateResult)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while creating the course, please try again",
                        null
                    )
                );
            }



            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Course created successfully",
                    null
                )
            );

        }

        [HttpGet("check-title")]
        public async Task<IActionResult> CheckCourseTitle([FromQuery] string title)
        {
            var courseTitleTaken = await _courseRepository.CourseExistsByTitle(title);

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    courseTitleTaken ? "Course title already taken" : "",
                    courseTitleTaken
                )
            );



        }
    }
}