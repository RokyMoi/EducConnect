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
            var courses = await _courseRepository.GetCoursesByQuery(requestSearchQuery, Request.Scheme, Request.Host.ToString());

            return Ok(
                ApiResponse<List<GetCoursesByQueryResponse>>.GetApiResponse(
                    "Courses retrieved successfully",
                    courses
                )
            );
        }
    }
}