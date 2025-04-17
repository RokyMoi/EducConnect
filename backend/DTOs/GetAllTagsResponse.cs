using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetAllTagsPaginatedResponse
    {
        public Guid TagId { get; set; }
        public string Name { get; set; }

        public int CoursesCount { get; set; }
        public Guid? CourseId { get; set; }
    }
}