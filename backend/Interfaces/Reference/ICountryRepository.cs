using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Reference.Country;

namespace backend.Interfaces.Reference
{
    public interface ICountryRepository
    {
        public Task<Country> GetCountryById(Guid countryId);
        public Task<List<Country>> GetAllCountries();
        public Task<Country> GetCountryByOfficialNameOrName(string name);
        public Task<Country> GetCountryByShorthandCode(string isoAlpha2Code);
        public Task<Country> GetCountryByNationalCallingCode(string nationalCallingCode);

        public Task AddCountriesToDatabase(List<Country> countries);

        public Task<bool> IsEmpty();

    }
}