using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Learning;
using backend.Entities.Reference.Learning;
using MimeKit.Cryptography;

namespace backend.DTOs.Learning
{
    public class LearningCategoryListAndLearningSubcategoryListDTO
    {
        public List<Entities.Learning.LearningCategory>
        LearningCategoriesList
        { get; set; }

        public List<EduConnect.Entities.Learning.LearningSubcategory>
        LearningSubcategoriesList
        { get; set; }
    }
}