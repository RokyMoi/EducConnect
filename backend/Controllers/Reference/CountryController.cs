using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.DTOs.Country;
using backend.Interfaces.Reference;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace backend.Controllers.Reference
{
    [ApiController]
    [Route("/country")]
    public class CountryController(ICountryRepository _countryRepository) : ControllerBase
    {

        [HttpGet("/country/all")]
        public async Task<IActionResult> GetAllCountries()
        {
            var countryList = await _countryRepository.GetAllCountries();

            if (countryList == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "No countries found",
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            var countryDTOList = new List<CountryResponseDTO>();

            foreach (var country in countryList)
            {
                var newCountryDTO = new CountryResponseDTO
                {
                    OfficialName = country.OfficialName,
                    CommonName = country.CommonName,
                    NationalCallingCode = "+" + country.NationalCallingCode,
                    ShorthandCode = country.ISOAlpha2Code,
                    FlagEmoji = country.FlagEmoji
                };

                countryDTOList.Add(newCountryDTO);
            }

            return Ok(new
            {
                success = "true",
                message = "Found data for " + countryList.Count + " countries",
                data = new { countries = countryDTOList },
                timestamp = DateTime.Now
            });
        }

        [HttpGet("/country/name/{countryName}")]
        public async Task<IActionResult> GetCountryByName([FromRoute] string countryName)
        {
            var country = await _countryRepository.GetCountryByOfficialNameOrName(countryName);

            if (country == null)
            {
                return NotFound(new
                {
                    success = "false",
                    message = "No country found with name " + countryName,
                    data = new { },
                    timestamp = DateTime.Now
                });
            }

            var countryResponseDTO = new CountryResponseDTO
            {
                OfficialName = country.OfficialName,
                CommonName = country.CommonName,
                NationalCallingCode = "+" + country.NationalCallingCode,
                ShorthandCode = country.ISOAlpha2Code,
                FlagEmoji = country.FlagEmoji
            };

            return Ok(new
            {
                success = "true",
                message = "Found data for country " + countryName,
                data = new { country = countryResponseDTO },
                timestamp = DateTime.Now
            });
        }

    }
}




