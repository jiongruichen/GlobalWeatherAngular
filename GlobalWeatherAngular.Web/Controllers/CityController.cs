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
    [RoutePrefix("api/city")]
    public class CityController : ApiBaseController
    {
        private readonly GlobalWeatherService.GlobalWeatherSoap _client;

        public CityController(GlobalWeatherService.GlobalWeatherSoap client)
        {
            _client = client;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("list")]
        public HttpResponseMessage Get(HttpRequestMessage request, string countryName)
        {
            var data = _client.GetCitiesByCountry(countryName);

            var cities = GetCities(data);

            var response = request.CreateResponse(HttpStatusCode.OK, cities);

            return response;
        }

        private List<string> GetCities(string xml)
        {
            var ret = new List<string>();
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            foreach (XmlNode node in doc.SelectNodes("//City"))
            {
                ret.Add(node.InnerText);
            }

            return ret;
        }
    }
}