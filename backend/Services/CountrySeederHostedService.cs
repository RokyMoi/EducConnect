using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Extensions;

namespace backend.Services
{
    public class CountrySeederHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public CountrySeederHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var countryExtractor = scope.ServiceProvider.GetRequiredService<CountryExtractor>();

                string filePath = "../backend/countries.json";

                var countries = CountryExtractor.ExtractCountriesFromJsonFile(filePath);
                await countryExtractor.SaveCountriesToDatabase(countries);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}