using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetAllCourseCategoriesResponse
    {

        public Guid CourseCategoryId { get; set; }
        public required string Name { get; set; }
    }
}