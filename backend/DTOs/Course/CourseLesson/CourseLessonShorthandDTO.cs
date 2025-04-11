using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs.Course.CourseLesson
{
    public class CourseLessonShorthandDTO
    {
        public Guid CourseLessonId { get; set; }

        public Guid CourseId { get; set; }

        public string LessonTitle { get; set; }

        public string LessonTag { get; set; }

        public int LessonSequenceOrder { get; set; }

        public int CourseLessonSupplementaryMaterialCount { get; set; }

        public long CourseLessonSupplementaryMaterialTotalSize { get; set; }

        public long CreatedAt { get; set; }


    }
}