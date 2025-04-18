using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Enums;

namespace EduConnect.DTOs
{
    public class GetCourseRequirementsByCourseIdResponseFromRepository
    {
        public Guid CourseId { get; set; }
        public decimal Price { get; set; }
        public bool IsCoursePayed { get; set; } = false;
        public int? MaxNumberOfStudents { get; set; }

        public PublishedStatus PublishedStatus { get; set; }
    }
}