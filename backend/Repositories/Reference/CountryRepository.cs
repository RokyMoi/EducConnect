using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Reference.Country;
using backend.Interfaces.Reference;
using EduConnect.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Reference
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _databaseContext;

        public CountryRepository(DataContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task AddCountriesToDatabase(List<Country> countries)
        {

            try
            {

                var isDatabaseEmpty = await _databaseContext.Country.FirstOrDefaultAsync();
                if (isDatabaseEmpty != null)
                {
                    return;
                }

                await _databaseContext.Country.AddRangeAsync(countries);
                await _databaseContext.SaveChangesAsync();
                Console.WriteLine($"Successfully added {countries.Count} countries to the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding countries: {ex.Message}");

            }
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await _databaseContext.Country.ToListAsync();
        }

        public async Task<Country> GetCountryById(Guid countryId)
        {
            var country = await _databaseContext.Country.FindAsync(countryId);

            if (country == null)
            {
                return null;
            }
            return country;
        }

        public async Task<Country> GetCountryByNationalCallingCode(string nationalCallingCode)
        {
            var country = await _databaseContext.Country.Where(x => x.NationalCallingCode.Equals(nationalCallingCode)).FirstOrDefaultAsync();

            if (country == null)
            {
                return null;
            }
            return country;
        }

        public Task<Country> GetCountryByName(string name)
        {
            var country = _databaseContext.Country.Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();

            if (country == null)
            {
                return null;
            }
            return country;
        }

        public Task<Country> GetCountryByShorthandCode(string isoAlpha2Code)
        {
            var country = _databaseContext.Country.Where(x => x.ISOAlpha2Code.Equals(isoAlpha2Code)).FirstOrDefaultAsync();

            if (country == null)
            {
                return null;
            }
            return country;
        }

        public async Task<bool> IsEmpty()
        {
            return await _databaseContext.Country.AnyAsync();
        }
    }
}