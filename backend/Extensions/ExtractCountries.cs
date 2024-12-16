using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Reference.Country;
using backend.Interfaces.Reference;
using Newtonsoft.Json;

namespace backend.Extensions
{
    public class CountryExtractor
    {
        private readonly ICountryRepository _countryRepository;
        public CountryExtractor(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public static List<Country> ExtractCountriesFromJsonFile(string jsonFilePath)
        {
            string json = File.ReadAllText(jsonFilePath);

            var countries = JsonConvert.DeserializeObject<List<dynamic>>(json);

            var countryList = new List<Country>();

            foreach (var country in countries)
            {


                Console.WriteLine(country.name);



                var newCountry = new Country
                {
                    CountryId = Guid.NewGuid(),
                    Name = country.name,
                    ISOAlpha2Code = country.code,
                    NationalCallingCode = country.dial_code,
                    FlagEmoji = country.emoji,
                };

                countryList.Add(newCountry);
            }
            return countryList;
        }


        public async Task SaveCountriesToDatabase(List<Country> countries)
        {

            try
            {
                await _countryRepository.AddCountriesToDatabase(countries);
                Console.WriteLine("Added  countries to database");
            }
            catch (System.Exception ex)
            {

                throw new Exception("Error saving countries to database, error: " + ex.Message);
            }
        }
    }
}
