using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Course.CourseLesson;
using EduConnect.Entities.Course;

namespace EduConnect.DTOs.Course.CourseLesson
{
    public class CourseLessonWithContentAndSupplementaryMaterialsDTO
    {
        public Guid TutorId { get; set; }

        public Guid CourseId { get; set; }

        public CourseLessonDTO CourseLesson { get; set; }

        public CourseLessonContentDTO CourseLessonContent { get; set; }

        public List<CourseLessonSupplementaryMaterialWithNoFileDTO> CourseLessonSupplementaryMaterials { get; set; }
    }
}