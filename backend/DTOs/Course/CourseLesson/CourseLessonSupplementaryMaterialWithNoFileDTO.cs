using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Course.CourseLesson
{
    public class CourseLessonSupplementaryMaterialWithNoFileDTO
    {
        public Guid CourseLessonId { get; set; }

        public Guid CourseLessonSupplementaryMaterialId { get; set; }
        public string ContentType { get; set; }

        public long ContentSize { get; set; }

        public string FileName { get; set; }

        public long DateTimePointOfFileCreation { get; set; }

        public long CreatedAt { get; set; }
    }
}