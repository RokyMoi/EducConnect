using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class UploadCourseTeachingResourceRequest
    {
        public Guid? CourseTeachingResourceId { get; set; } = null;
        public Guid? CourseId { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MinLength(25)]
        [MaxLength(255)]
        public string Description { get; set; }

        [RegularExpression(@"[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)")]
        public string? ResourceUrl { get; set; } = null;

        public IFormFile? ResourceFile { get; set; } = null;




    }
}