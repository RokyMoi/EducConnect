using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class UploadCourseThumbnailRequest
    {
        public Guid CourseId { get; set; }

        public bool UseAzureStorage { get; set; } = false;
        public IFormFile ThumbnailData { get; set; }
    }
}