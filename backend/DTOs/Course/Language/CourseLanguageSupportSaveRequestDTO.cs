using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Course.Language
{
    public class CourseLanguageSupportSaveRequestDTO
    {
        public Guid CourseId { get; set; }
        public Guid LanguageId { get; set; }
    }
}