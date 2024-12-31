using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Extensions;

namespace backend.Services
{
    public class LearningCategoryAndSubcategoryHostedService : IHostedService
    {

        private readonly IServiceProvider _serviceProvider;
        public LearningCategoryAndSubcategoryHostedService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var learningCategoryAndSubcategoryExtractor = scope.ServiceProvider.GetRequiredService<ExtractLearningCategoriesAndSubcategories>();

                string filePath = "../backend/learning_categories_subcategories.json";

                var learningCategoriesAndSubcategories = ExtractLearningCategoriesAndSubcategories.ExtractLearningCategoriesFromJsonFile(filePath);

                await learningCategoryAndSubcategoryExtractor.SaveLearningCategoriesAndLearningSubcategoriesToDatabase(learningCategoriesAndSubcategories);

            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}