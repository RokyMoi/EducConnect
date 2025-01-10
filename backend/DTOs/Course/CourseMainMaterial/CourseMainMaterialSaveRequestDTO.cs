using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Course.CourseMainMaterial
{
    public class CourseMainMaterialSaveRequestDTO
    {
        public Guid CourseId { get; set; }
        public string FileName { get; set; }

        public DateTime DateTimePointOfFileCreation { get; set; }

        public IFormFile FileToUpload { get; set; }
    }
}