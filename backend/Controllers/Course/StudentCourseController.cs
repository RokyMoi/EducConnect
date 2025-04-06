using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces.Tutor;
using EduConnect.DTOs;
using EduConnect.Interfaces;
using EduConnect.Interfaces.Course;
using EduConnect.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Controllers.Course
{
    [ApiController]
    [Route("student/course")]
    [AuthenticationGuard(isTutor: false, isAdmin: false, isStudent: true)]
    public class StudentCourseController(
        ITutorRepository tutorRepository,
        IStudentRepository _studentRepository,
        ICourseRepository _courseRepository

    ) : ControllerBase
    {

    }
}