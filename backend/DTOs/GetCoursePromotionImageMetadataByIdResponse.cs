using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetCoursePromotionImageMetadataByIdResponse
    {
        public Guid CoursePromotionImageId { get; set; }
        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}