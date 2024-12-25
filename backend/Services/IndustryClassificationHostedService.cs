using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Extensions;

namespace backend.Services
{
    public class IndustryClassificationHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;


        public IndustryClassificationHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var industryClassificationExtractor = scope.ServiceProvider.GetRequiredService<ExtractIndustryClassification>();

                string filePath = "../backend/industry_classification.json";

                var industryClassification = ExtractIndustryClassification.ExtractIndustryClassificationFromJsonFile(filePath);
                await industryClassificationExtractor.SaveIndustryClassificationToDatabase(industryClassification);


            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}