using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces.Tutor;
using EduConnect.DTOs;
using EduConnect.Entities;
using EduConnect.Interfaces;
using EduConnect.Interfaces.Course;
using EduConnect.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Controllers.Course
{
    [ApiController]
    [Route("student/course")]
    [AuthenticationGuard(isTutor: false, isAdmin: false, isStudent: true)]
    public class CourseStudentController(
        ITutorRepository tutorRepository,
        IStudentRepository _studentRepository,
        ICourseRepository _courseRepository

    ) : ControllerBase
    {
        [HttpGet("search")]
        public async Task<IActionResult> GetCoursesByQuery([FromQuery] SearchCoursesQueryRequest request)
        {
            var requestSearchQuery = string.IsNullOrEmpty(request.SearchQuery) ? "" : request.SearchQuery;
            var (courses, totalCount) = await _courseRepository.GetCoursesByQuery(requestSearchQuery, Request.Scheme, Request.Host.ToString(), request.PageNumber, request.PageSize);

            return Ok(
                ApiResponse<List<GetCoursesByQueryResponse>>.GetApiPaginatedResponse(
                    "Courses retrieved successfully",
                    courses,
                    totalCount,
                    request.PageNumber,
                    request.PageSize

                )
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseById([FromQuery] Guid courseId)
        {
            var course = await _courseRepository.GetCourseByIdForStudent(courseId, Request.Scheme, Request.Host.ToString());
            if (course == null)
            {
                return NotFound(new ApiResponse<object>("Course not found", null));
            }

            return Ok(new ApiResponse<GetCoursesByQueryResponse>("Course retrieved successfully", course));
        }
    }
}