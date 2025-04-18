using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Enums;

namespace EduConnect.DTOs
{
    public class ChangeCourseLessonPublishedStatusRequest
    {
        public Guid CourseLessonId { get; set; }
        [Range(1, int.MaxValue)]
        public int? LessonSequenceOrder { get; set; } = null;

    }
}