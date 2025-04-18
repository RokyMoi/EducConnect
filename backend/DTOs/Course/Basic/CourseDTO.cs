using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Course.Basic
{
    public class CourseDTO
    {
        public Guid CourseId { get; set; }
        public Guid TutorId { get; set; }

        public string CourseName { get; set; }

        public string CourseSubject { get; set; }

        public Guid CourseCreationCompletenessStepId { get; set; }
    }
}