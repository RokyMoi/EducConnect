using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces.Reference;
using backend.Interfaces.Tutor;
using EduConnect.DTOs;
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

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCourses()
        {
            var personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());

            var tutor = await _tutorRepository.GetTutorByPersonId(personId);

            if (tutor == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while getting the courses, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }


            var courses = await _courseRepository.GetAllCoursesByTutorId(tutor.TutorId);

            if (courses == null || courses.Count == 0)
            {
                return NoContent();
            }

            var response = courses.Select(
                x => new GetAllCoursesResponse
                {
                    CourseId = x.CourseId,
                    Title = x.Title,
                    Description = x.Description,
                    CourseCategoryId = x.CourseCategoryId,
                    CourseCategoryName = x.CourseCategory.Name,
                    LearningDifficultyLevelId = x.LearningDifficultyLevelId,
                    LearningDifficultyLevelName = x.LearningDifficultyLevel.Name,
                    MinNumberOfStudents = x.MinNumberOfStudents,
                    MaxNumberOfStudents = x.MaxNumberOfStudents,
                    Price = x.Price,
                    PublishedStatus = x.PublishedStatus,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).UtcDateTime,
                });

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Courses retrieved successfully",
                    response
                )
            );



        }

        [HttpGet("info")]
        public async Task<IActionResult> GetCourseManagementDashboardInfo(
            [FromQuery] Guid courseId
        )
        {
            var personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());

            var tutor = await _tutorRepository.GetTutorByPersonId(personId);

            if (tutor == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while getting the courses, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }

            var course = await _courseRepository.GetCourseById(courseId);

            if (course == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Course not found",
                        null
                    )
                );

            }

            var response = new CourseManagementDashboardResponse
            {
                CourseId = course.CourseId,
                Title = course.Title,
                DifficultyLevel = course.LearningDifficultyLevel.Name,
                Category = course.CourseCategory.Name,
                CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(course.CreatedAt).UtcDateTime,
                UpdatedAt = course.UpdatedAt != null ? DateTimeOffset.FromUnixTimeMilliseconds((long)course.UpdatedAt).UtcDateTime : null,
            };

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Course retrieved successfully",
                    response
                )
            );

        }

        [HttpGet("basics")]
        public async Task<IActionResult> GetCourseBasics([FromQuery] Guid courseId)
        {
            var personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());

            var tutor = await _tutorRepository.GetTutorByPersonId(personId);

            if (tutor == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while getting the courses, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }

            var course = await _courseRepository.GetCourseById(courseId);

            if (course == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Course not found",
                        null
                    )
                );
            }

            var response = new GetAllCoursesResponse
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                CourseCategoryId = course.CourseCategoryId,
                CourseCategoryName = course.CourseCategory.Name,
                LearningDifficultyLevelId = course.LearningDifficultyLevelId,
                LearningDifficultyLevelName = course.LearningDifficultyLevel.Name,
                MinNumberOfStudents = course.MinNumberOfStudents,
                MaxNumberOfStudents = course.MaxNumberOfStudents,
                Price = course.Price,
                PublishedStatus = course.PublishedStatus,
                CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(course.CreatedAt).UtcDateTime,

            };

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Course retrieved successfully",
                    response
                )
            );
        }

        [HttpPatch("update/basics")]
        public async Task<IActionResult> UpdateCourseBasics(
            [FromBody] UpdateCourseBasicsRequest request
        )
        {
            var personId = Guid.Parse(HttpContext.Items["PersonId"].ToString());

            var tutor = await _tutorRepository.GetTutorByPersonId(personId);

            if (tutor == null)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while getting the courses, please refer to your administrator, regarding your role",
                        null
                    )
                );
            }

            //Check if the course exists and if the tutor is the owner of the course
            var course = await _courseRepository.GetCourseById(request.CourseId);

            if (course == null)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Course not found",
                        null
                    )
                );
            }

            //Check if the course title is taken by another course
            var courseTitleExists = await _courseRepository.CourseExistsByTitleExceptTheGivenCourseById(request.CourseId, request.Title);

            if (courseTitleExists)
            {
                return Conflict(
                    ApiResponse<object>.GetApiResponse(
                        "Course title already exists",
                        null
                    )
                );
            }


            if (course.TutorId != tutor.TutorId)
            {
                return Conflict(
                    ApiResponse<object>.GetApiResponse(
                        "You are not authorized to update this course",
                        null
                    )
                );
            }

            //Check if the course category exists
            var courseCategory = await _referenceRepository.CourseCategoryExistsById(request.CourseCategoryId);

            if (!courseCategory)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Course category not found",
                        null
                    )
                );
            }


            //Check if the learning difficulty level exists
            var learningDifficultyLevel = await _referenceRepository.LearningDifficultyLevelExistsById(request.LearningDifficultyLevelId);

            if (!learningDifficultyLevel)
            {
                return NotFound(
                    ApiResponse<object>.GetApiResponse(
                        "Learning difficulty level not found",
                        null
                    )
                );
            }

            //Update the course 
            course.Title = request.Title;
            course.Description = request.Description;
            course.CourseCategoryId = request.CourseCategoryId;
            course.LearningDifficultyLevelId = request.LearningDifficultyLevelId;
            course.MinNumberOfStudents = request.MinNumberOfStudents;
            course.MaxNumberOfStudents = request.MaxNumberOfStudents;
            course.Price = request.Price;
            course.PublishedStatus = request.PublishedStatus;
            course.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var updateResult = await _courseRepository.UpdateCourseBasics(course);

            if (!updateResult)
            {
                return StatusCode(
                    500,
                    ApiResponse<object>.GetApiResponse(
                        "An error occurred while updating the course, please try again later",
                        null
                    )
                );
            }

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    "Course updated successfully",
                    null
                )
            );


        }

        [HttpGet("check-title-except")]
        public async Task<IActionResult> CheckCourseTitleExistEmitGivenCourse(
            [FromQuery] CheckCourseTitleExistEmitGivenCourseRequest request
        )
        {
            var courseTitleExists = await _courseRepository.CourseExistsByTitleExceptTheGivenCourseById(request.CourseId, request.Title);

            return Ok(
                ApiResponse<object>.GetApiResponse(
                    $"Course title {request.Title} {(courseTitleExists ? "already exists" : "does not exist")}",
                    courseTitleExists
                )
            );
        }
    }
}