using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class UpdateCourseLessonResourceMetadataRequest
    {
        public Guid CourseLessonResourceId { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MinLength(25)]
        [MaxLength(255)]
        public string Description { get; set; }
    }
}