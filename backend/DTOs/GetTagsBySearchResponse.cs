using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetTagsBySearchResponse
    {
        public Guid TagId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CourseCount { get; set; } = 0;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsAssigned { get; set; }



    }
}