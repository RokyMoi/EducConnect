using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Course;

namespace backend.DTOs.Course.Basic
{
    public class CourseAndCourseTypeDTO
    {
        public Guid CourseId { get; set; }
        public CourseType CourseType { get; set; }
    }
}