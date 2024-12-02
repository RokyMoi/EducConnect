using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace backend.Controllers.Reference
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CountryController(HttpClient httpClient)
        {
            this._httpClient = httpClient;
            this._httpClient.Timeout = TimeSpan.FromMinutes(10);

        }



    }
}