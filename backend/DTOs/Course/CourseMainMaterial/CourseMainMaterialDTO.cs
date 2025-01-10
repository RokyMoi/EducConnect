using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Course.CourseMainMaterial
{
    public class CourseMainMaterialDTO
    {
        public Guid CourseMainMaterialId { get; set; }
        public Guid CourseId { get; set; }
        public string FileName { get; set; }

        public string ContentType { get; set; }

        public long ContentSize { get; set; }

        public byte[] Data { get; set; }

        public DateTime DateTimePointOfFileCreation { get; set; }
    }
}