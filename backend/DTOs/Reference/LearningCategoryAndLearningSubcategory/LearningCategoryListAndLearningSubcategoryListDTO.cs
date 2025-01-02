using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Learning;
using MimeKit.Cryptography;

namespace backend.DTOs.Learning
{
    public class LearningCategoryListAndLearningSubcategoryListDTO
    {
        public List<LearningCategory>
        LearningCategoriesList
        { get; set; }

        public List<LearningSubcategory>
        LearningSubcategoriesList
        { get; set; }
    }
}