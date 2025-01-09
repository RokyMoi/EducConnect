using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Learning;

namespace EduConnect.Entities.Learning
{
    [Table("LearningSubcategory", Schema = "Reference")]
    public class LearningSubcategory
    {
        [Key]
        public required Guid LearningSubcategoryId { get; set; }
        public required Guid LearningCategoryId { get; set; }
        //Navigation property
        [ForeignKey(nameof(LearningCategoryId))]
        public LearningCategory? LearningCategory { get; set; }
        public required string LearningSubcategoryName { get; set; }
        public string? Description { get; set; } = null;
        public long CreatedAt { get; set; }
        public long? UpdatedAt { get; set; } = null;
    }
}