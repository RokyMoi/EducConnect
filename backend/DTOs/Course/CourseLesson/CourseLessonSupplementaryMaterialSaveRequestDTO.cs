using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs.Course.CourseLesson
{
    public class CourseLessonSupplementaryMaterialSaveRequestDTO
    {
        public Guid CourseLessonId { get; set; }
        public string FileName { get; set; }
        public long DateTimePointOfFileCreation { get; set; }
        public IFormFile FileToUpload { get; set; }
    }
}