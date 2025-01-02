using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Course.Basic
{
    public class CourseCreateDTO
    {
        public Guid TutorId { get; set; }
        public string CourseName { get; set; }
        public string CourseSubject { get; set; }

    }
}