using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class AssignTagToCourseRequest
    {
        public Guid CourseId { get; set; }
        public Guid TagId { get; set; }
    }
}