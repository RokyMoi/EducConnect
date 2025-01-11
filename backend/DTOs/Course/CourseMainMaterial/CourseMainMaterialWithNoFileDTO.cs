using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Course.CourseMainMaterial
{
    public class CourseMainMaterialWithNoFileDTO
    {
        public Guid CourseMainMaterialId { get; set; }
        public Guid CourseId { get; set; }
        public string FileName { get; set; }

        public string ContentType { get; set; }

        public long ContentSize { get; set; }

        public long DateTimePointOfFileCreation { get; set; }
    }
}