using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Reference.LearningDifficultyLevel
{
    public class LearningDifficultyLevelDTO
    {
        public int LearningDifficultyLevelId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}