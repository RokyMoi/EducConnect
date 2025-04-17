using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetAllCourseTagsByCourseId
    {
        public Guid CourseTagId { get; set; }
        public Guid TagId { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; }

    }
}