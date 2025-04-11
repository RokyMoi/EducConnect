using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetCoursePromotionImagesMetadataResponse
    {
        public Guid CoursePromotionImageId { get; set; }

        public Guid CourseId { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}