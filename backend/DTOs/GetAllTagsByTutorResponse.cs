using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetAllTagsByTutorResponse
    {
        public Guid TagId { get; set; }
        public string Name { get; set; }
        public int CourseCount { get; set; }
        public Guid TutorId { get; set; }


    }
}