using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class UploadCourseTeachingResourceRequest
    {
        public Guid? CourseTeachingResourceId { get; set; } = null;
        public Guid CourseId { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MinLength(25)]
        [MaxLength(255)]
        public string Description { get; set; }

        public string? ResourceUrl { get; set; } = null;

        public IFormFile? ResourceFile { get; set; } = null;




    }
}