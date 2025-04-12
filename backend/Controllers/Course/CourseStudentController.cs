using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Interfaces.Person;
using backend.Interfaces.Tutor;
using EduConnect.DTOs;
using EduConnect.Entities;
using EduConnect.Entities.Course;
using EduConnect.Interfaces;
using EduConnect.Interfaces.Course;
using EduConnect.Middleware;
using EduConnect.SignalIR;
using EduConnect.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace EduConnect.Controllers.Course
{
    [ApiController]
    [Route("student/course")]
    [AuthenticationGuard(isTutor: false, isAdmin: false, isStudent: true)]
    public class CourseStudentController(
        ITutorRepository tutorRepository,
        IStudentRepository _studentRepository,
        ICourseRepository _courseRepository,
        IPersonRepository _personRepository,
        IHttpContextAccessor _httpContextAccessor,
        IHubContext<CourseAnalyticsHub> _courseAnalyticsHubContext

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

        [HttpPost("analytics")]
        public async Task<IActionResult> AddCourseViewershipData([FromBody] AddCourseViewershipDataRequest request)
        {
            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            var nameIdentifierClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Console.WriteLine(nameIdentifierClaim);

            if (string.IsNullOrEmpty(nameIdentifierClaim.Value) || !Guid.TryParse(nameIdentifierClaim.Value, out var publicPersonId))
            {
                return Ok();
            }

            var person = await _personRepository.GetPersonByPublicPersonId(Guid.Parse(nameIdentifierClaim.Value));

            var courseViewershipData = new CourseViewershipData
            {
                ViewedByPersonId = person.PersonId,
                ClickedOn = request.ClickedOn,
                CourseId = request.CourseId,
                UserCameFrom = request.UserCameFrom,

            };

            Console.WriteLine($"Add CourseViewershipData for " + person.PersonId + " for course " + request.CourseId + " with following properties: ");
            PrintObjectUtility.PrintObjectProperties(courseViewershipData);

            var createResult = await _courseRepository.CreateCourseViewershipData(courseViewershipData);
            return Ok(
                ApiResponse<Guid>.GetApiResponse(
                    "Course viewership data added successfully",
                    courseViewershipData.CourseViewershipDataId
                )
            );
        }

        [HttpPatch("analytics/entered")]
        public async Task<IActionResult> SetEnteredOnCourseViewershipData([FromQuery] Guid courseViewershipDataId)
        {
            var courseViewershipData = await _courseRepository.GetCourseViewershipDataById(courseViewershipDataId);

            if (courseViewershipData == null)
            {
                return Ok();
            }

            courseViewershipData.EnteredDetailsAt = DateTime.UtcNow;
            courseViewershipData.UpdatedAt = DateTime.UtcNow.ToUnixTimeMilliseconds();

            Console.WriteLine("Set Student CourseViewershipData EnteredOn: " + courseViewershipData.EnteredDetailsAt + " for " + courseViewershipData.CourseViewershipDataId);
            var updateResult = await _courseRepository.UpdateCourseViewershipData(courseViewershipData);

            Console.WriteLine("Update course viewership data result: " + updateResult);


            return Ok();

        }

        [HttpPatch("analytics/exited")]
        public async Task<IActionResult> SetLeftOnCourseViewershipData([FromQuery] Guid courseViewershipDataId)
        {
            var courseViewershipData = await _courseRepository.GetCourseViewershipDataById(courseViewershipDataId);

            if (courseViewershipData == null)
            {
                return Ok();
            }

            courseViewershipData.LeftDetailsAt = DateTime.UtcNow;
            courseViewershipData.UpdatedAt = DateTime.UtcNow.ToUnixTimeMilliseconds();
            var updateResult = await _courseRepository.UpdateCourseViewershipData(courseViewershipData);

            Console.WriteLine("Update course viewership data result: " + updateResult);

            return Ok();

        }
    }
}