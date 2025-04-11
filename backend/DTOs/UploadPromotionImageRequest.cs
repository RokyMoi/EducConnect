using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class UploadPromotionImageRequest
    {
        public Guid? CourseId { get; set; }
        public Guid? CoursePromotionImageId { get; set; }
        public IFormFile Image { get; set; }
    }
}