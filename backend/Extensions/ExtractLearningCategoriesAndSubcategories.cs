using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Learning;
using backend.Entities.Learning;
using backend.Interfaces.Reference;
using Newtonsoft.Json;

namespace backend.Extensions
{
    public class ExtractLearningCategoriesAndSubcategories
    {
        private readonly IReferenceRepository _referenceRepository;

        public ExtractLearningCategoriesAndSubcategories(IReferenceRepository referenceRepository)
        {
            this._referenceRepository = referenceRepository;
        }

        public static LearningCategoryListAndLearningSubcategoryListDTO
        ExtractLearningCategoriesFromJsonFile(string jsonFilePath)
        {
            string json = File.ReadAllText(jsonFilePath);

            var learningCategories = JsonConvert.DeserializeObject<List<dynamic>>(json);

            var learningCategoriesList = new List<LearningCategory>();

            var learningSubcategoriesList = new List<LearningSubcategory>();

            foreach (var category in learningCategories)
            {

                LearningCategory newLearningCategory = new()
                {
                    LearningCategoryId = Guid.NewGuid(),
                    LearningCategoryName = category.Category.Name,
                    LearningCategoryDescription = category.Category.Description
                };
                learningCategoriesList.Add(newLearningCategory);
                foreach (var subcategory in category.Subcategories)
                {
                    LearningSubcategory newLearningSubcategory = new()
                    {
                        LearningSubcategoryId = Guid.NewGuid(),
                        LearningCategoryId = newLearningCategory.LearningCategoryId,
                        LearningSubcategoryName = subcategory.Name,
                        Description = subcategory.Description,
                        CreatedAt = DateTimeOffset.Now.ToUnixTimeSeconds(),
                        UpdatedAt = null,
                    };
                    learningSubcategoriesList.Add(newLearningSubcategory);
                }
            }




            return new LearningCategoryListAndLearningSubcategoryListDTO
            {
                LearningCategoriesList = learningCategoriesList,
                LearningSubcategoriesList = learningSubcategoriesList
            };
        }

        public async Task SaveLearningCategoriesAndLearningSubcategoriesToDatabase(
            LearningCategoryListAndLearningSubcategoryListDTO learningCategoriesAndSubcategoriesToSave
        )
        {
            try
            {
                await _referenceRepository.AddLearningCategoriesToDatabase(
                    learningCategoriesAndSubcategoriesToSave.LearningCategoriesList
                );
                await _referenceRepository.AddLearningSubcategoriesToDatabase(
                    learningCategoriesAndSubcategoriesToSave.LearningSubcategoriesList
                );
                Console.WriteLine("Learning Categories should be added to the database");

            }
            catch (System.Exception ex)
            {

                throw new Exception("Error saving learning categories to database, error: " + ex.Message);
            }
        }
    }
}