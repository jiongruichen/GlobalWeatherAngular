﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace GlobalWeatherAngular.Web
{
    public interface ICountryService
    {
        List<string> GetCountries();
    }

    public class CountryService : ICountryService
    {
        public CountryService()
        {

        }

        public List<string> GetCountries()
        {
            var ret = new List<string>();
            var doc = new XmlDocument();

            doc.Load(HttpContext.Current.Server.MapPath("~/Countries.xml"));

            foreach (XmlNode node in doc.SelectNodes("//country"))
            {
                ret.Add(node.InnerText);
            }

            return ret;
        }
    }
}