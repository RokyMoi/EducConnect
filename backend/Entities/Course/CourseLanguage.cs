using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Reference.Language;
using Microsoft.EntityFrameworkCore;

namespace backend.Entities.Course
{
    [Table("CourseLanguage", Schema = "Course")]
    public class CourseLanguage
    {

        public Guid CourseId { get; set; }

        [ForeignKey("CourseId")]
        //Navigation property
        public EduConnect.Entities.Course.Course? Course { get; set; }

        public Guid LanguageId { get; set; }

        [ForeignKey("LanguageId")]
        //Navigation property 
        public Language? Language { get; set; }

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public long? UpdatedAt { get; set; } = null;
    }
}