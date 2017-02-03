using Autofac;
using Autofac.Integration.WebApi;
using GlobalWeatherAngular.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using GlobalWeatherAngular.Web.Services;

namespace GlobalWeatherAngular.Web.App_Start
{
    public class AutofacWebapiConfig
    {
        public static IContainer Container;
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<GlobalWeatherService.GlobalWeatherSoapClient>()
                .As<GlobalWeatherService.GlobalWeatherSoap>()
                .InstancePerRequest();

            builder.RegisterType<CountryService>()
                .As<ICountryService>()
                .InstancePerRequest();

            builder.RegisterType<WeatherMapService>()
                .As<IWeatherMapService>()
                .InstancePerRequest();
            
            Container = builder.Build();

            return Container;
        }
    }
}