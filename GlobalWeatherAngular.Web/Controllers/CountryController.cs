using GlobalWeatherAngular.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace GlobalWeatherAngular.Web.Controllers
{
    [RoutePrefix("api/country")]
    public class CountryController : ApiBaseController
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("list")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            var countries = _countryService.GetCountries();

            var response = request.CreateResponse(HttpStatusCode.OK, countries);

            return response;
        }
    }
}