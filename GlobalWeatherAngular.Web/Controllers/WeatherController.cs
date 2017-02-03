using GlobalWeatherAngular.Web.Infrastructure.Core;
using GlobalWeatherAngular.Web.Models;
using GlobalWeatherAngular.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace GlobalWeatherAngular.Web.Controllers
{
    [RoutePrefix("api/weather")]
    public class WeatherController : ApiBaseController
    {
        private IWeatherMapService _weatherMapService;

        public WeatherController(IWeatherMapService weatherMapService)
        {
            _weatherMapService = weatherMapService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("get")]
        public HttpResponseMessage Get(HttpRequestMessage request, string cityName)
        {
            var weatherMap = _weatherMapService.GetWeatherDetails(cityName).Result;

            var viewModel = new WeatherDetailViewModels();

            if (weatherMap != null)
            {
                viewModel.Location = string.Format("Lon: {0}, Lat: {1}", weatherMap.coord.lon, weatherMap.coord.lat);
                viewModel.Time = UnixTimeStampToDateTime(weatherMap.dt).ToString("dd/MM/yyyy HH:mm:ss");
                viewModel.Wind = string.Format("Speed: {0} metre/sec, Degree: {1}", weatherMap.wind.speed.ToString(), weatherMap.wind.deg.ToString());
                viewModel.Visibility = weatherMap.visibility.ToString();
                viewModel.SkyConditions = weatherMap.weather[0].description;
                viewModel.Temperature = ConvertFromKelvin(weatherMap.main.temp).ToString() + "°C";
                viewModel.RelativeHumidity = (weatherMap.main.humidity / 100).ToString("P");
                viewModel.Pressure = weatherMap.main.pressure.ToString() + " hPa";
            }

            var response = request.CreateResponse(HttpStatusCode.OK, viewModel);

            return response;
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();

            return dtDateTime;
        }

        private double ConvertFromKelvin(double kel)
        {
            return kel - 273;
        }
    }
}