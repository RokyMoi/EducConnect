using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Entities.Learning
{
    [Table("LearningCategory", Schema = "Reference")]
    public class LearningCategory
    {
        [Key]
        public Guid LearningCategoryId { get; set; }
        public string LearningCategoryName { get; set; }
        public string LearningCategoryDescription { get; set; }
        public long CreatedAt { get; set; }
        public long? ModifiedAt { get; set; }
    }
}