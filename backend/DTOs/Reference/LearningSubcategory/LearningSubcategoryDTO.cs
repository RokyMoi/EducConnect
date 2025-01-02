using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Reference.LearningSubcategory
{
    public class LearningSubcategoryDTO
    {
        public Guid LearningSubcategoryId { get; set; }
        public Guid LearningCategoryId { get; set; }
        public string LearningSubcategoryName { get; set; }
        public string? Description { get; set; } = null;
    }
}