using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.LearningCategory
{
    public class LearningCategoryDTO
    {
        public Guid LearningCategoryId { get; set; }
        public string LearningCategoryName { get; set; }
        public string LearningCategoryDescription { get; set; }
    }
}