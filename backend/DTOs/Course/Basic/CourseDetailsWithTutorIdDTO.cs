using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Course.Basic
{
    public class CourseDetailsWithTutorIdDTO
    {
        public Guid CourseId { get; set; }
        public string CourseDescription { get; set; }
        public double Price { get; set; }
        public Guid LearningSubcategoryId { get; set; }
        public int LearningDifficultyLevelId { get; set; }
        public int CourseTypeId { get; set; }
        public Guid TutorId { get; set; }
    }
}