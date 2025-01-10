using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Entities.Reference.Learning
{
    [Table("LearningDifficultyLevel", Schema = "Reference")]
    public class LearningDifficultyLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LearningDifficultyLevelId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

    }
}