using GlobalWeatherAngular.Web;
using GlobalWeatherAngular.Web.Controllers;
using GlobalWeatherAngular.Web.Models;
using GlobalWeatherAngular.Web.Services;
using GlobalWeatherAngular.Web.GlobalWeatherService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Net;

namespace GlobalWeatherAngular.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private Mock<GlobalWeatherSoap> client;
        private Mock<IWeatherMapService> weatherMapService;
        private Mock<ICountryService> countryService;
        private HttpRequestMessage request;

        [TestInitialize]
        public void Init()
        {
            client = new Mock<GlobalWeatherSoap>();
            weatherMapService = new Mock<IWeatherMapService>();
            countryService = new Mock<ICountryService>();

            request = new HttpRequestMessage();
            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
        }

        [TestMethod]
        public void Index()
        {
            var target = new HomeController();

            ViewResult result = target.Index() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetCountries()
        {
            var countries = new List<string> { "Australia", "United States" };
            countryService.Setup(s => s.GetCountries()).Returns(countries);
            
            var target = new CountryController(countryService.Object);
            HttpResponseMessage result = target.Get(request) as HttpResponseMessage;
            
            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public void GetCityNames()
        {
            var countryName = "Australia";
            var cities = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><string xmlns=\"http://www.webserviceX.NET\"><NewDataSet><Table><Country>Australia</Country><City>Sydney Airport</City></Table></NewDataSet></string>";
            client.Setup(s => s.GetCitiesByCountry(It.IsAny<string>())).Returns(cities);

            var target = new CityController(client.Object);
            HttpResponseMessage result = target.Get(request, countryName) as HttpResponseMessage;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public void GetWeatherDetails()
        {
            var cityName = "Sydney Airport";
            var data = "{\"coord\":{\"lon\":151.21,\"lat\":-33.87},\"weather\":[{\"id\":500,\"main\":\"Rain\",\"description\":\"light rain\",\"icon\":\"10n\"}],\"base\":\"stations\",\"main\":{\"temp\":295.291,\"pressure\":1022.65,\"humidity\":96,\"temp_min\":295.291,\"temp_max\":295.291,\"sea_level\":1028.67,\"grnd_level\":1022.65},\"wind\":{\"speed\":0.82,\"deg\":17.5047},\"rain\":{\"3h\":0.6075},\"clouds\":{\"all\":92},\"dt\":1486073421,\"sys\":{\"message\":0.0111,\"country\":\"AU\",\"sunrise\":1485976708,\"sunset\":1486025936},\"id\":2147714,\"name\":\"Sydney\",\"cod\":200}";

            var weatherDetails = new ServiceResult<WeatherMap>
            {
                Result = JsonConvert.DeserializeObject<WeatherMap>(data),
                Error = "",
                Success = true
            };

            weatherMapService.Setup(s => s.GetWeatherDetails(It.IsAny<string>())).Returns(weatherDetails);

            var target = new WeatherController(weatherMapService.Object);
            HttpResponseMessage result = target.Get(request, cityName) as HttpResponseMessage;

            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
        }
    }
}
