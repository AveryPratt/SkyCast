﻿using Newtonsoft.Json;
using SkyCast.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SkyCast.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Geolocation(string location, string dateTime)
        {
			TempData["location"] = location;
			try
			{
				TempData["dateTime"] = String.IsNullOrEmpty(dateTime) ? null : (DateTime?)Convert.ToDateTime(dateTime);
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = "Unrecognizable date";
				return this.RedirectToAction("Index");
			}
			string geoKey = "AIzaSyAkI1hZCvx9yR-e3YcFAGb9AiaVovAfPpA";
			string geoUri = String.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}", location, geoKey);
			// call Geo API
			try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(geoUri).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string geoJson = response.Content.ReadAsStringAsync().Result;
                        GeoReport geoReport = JsonConvert.DeserializeObject<GeoReport>(geoJson);
                        Location coordinates = geoReport.results.First().geometry.location;
						TempData["geoReport"] = geoReport;
                        return this.RedirectToAction("WeatherForecast");
                    }
					throw new HttpRequestException("Bad Request");
                }
            }
            catch (HttpRequestException)
			{
				TempData["errorMessage"] = "Could not find location.";
			}
			return this.RedirectToAction("Index");
		}

        public ActionResult WeatherForecast(JsonResult jsonResult)
        {
            string weatherKey = "319cbb1ff967b87cc559b2e8308d0ddc";
            string weatherUri;
			GeoReport geoReport = TempData["geoReport"] as GeoReport;
			DateTime? dateTime = (DateTime?)TempData["datetime"];

			try
			{
				Location coordinates = geoReport.results.First().geometry.location;
				weatherUri = String.Format("https://api.darksky.net/forecast/{0}/{1},{2}", weatherKey, coordinates.lat, coordinates.lng);
				if (dateTime != null)
				{
					weatherUri += "," + ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
				}
			}
            catch (Exception ex)
			{
				TempData["errorMessage"] = "Could not find coordinates for location.";
				return this.RedirectToAction("Index");
            }

            // call Weather API
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(weatherUri).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string weatherJson = response.Content.ReadAsStringAsync().Result;
						WeatherReport weatherReport = JsonConvert.DeserializeObject<WeatherReport>(weatherJson);
						TempData["weatherReport"] = weatherReport;
                    }
					else
					{
						throw new HttpRequestException("Bad Request");
					}
                }
            }
            catch (HttpRequestException)
            {
				TempData["errorMessage"] = "Could not find weather data.";
			}
			Query query = new Query()
			{
				location = TempData["location"] as String,
				geoReport = TempData["geoReport"] as GeoReport,
				weatherReport = TempData["weatherReport"] as WeatherReport
			};
			Session["query"] = query;
			return this.RedirectToAction("Index");
		}

		public ActionResult Index(ICollection<Query> queries = null)
		{
			if (User.Identity.IsAuthenticated && queries != null)
			{
				return this.RedirectToAction("Details", "Weather");
			}
			return View();
		}

		public ActionResult Index(ICollection<Query> queries)
		{
			return View(queries);
		}

		public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}