using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs.Course.CourseLesson
{
    public class CourseLessonSupplementaryMaterialCreateDTO
    {
        public Guid CourseLessonId { get; set; }

        public string ContentType { get; set; }

        public long ContentSize { get; set; }

        public string FileName { get; set; }

        public byte[] Data { get; set; }

        public long DateTimePointOfFileCreation { get; set; }
    }
}