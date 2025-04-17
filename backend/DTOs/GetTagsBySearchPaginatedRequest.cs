using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetTagsBySearchPaginatedRequest
    {
        public string? SearchQuery { get; set; }
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;
        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
        public Guid? ContainsTagCourseId { get; set; } // Add this


    }
}