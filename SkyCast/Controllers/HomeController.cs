using Newtonsoft.Json;
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
        public ActionResult Geolocation(string location)
        {
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
                        GeoResult geoResult = new GeoResult()
                        {
                            name = location,
							report = geoReport
                        };
						string jsonObj = new JavaScriptSerializer().Serialize(geoResult);
                        return this.RedirectToAction("WeatherForecast", Json(jsonObj));
                    }
                }
            }
            catch (Exception ex)
            {

			}
			return this.RedirectToAction("Index", (object)"Could not find location.");
		}

        public ActionResult WeatherForecast(JsonResult jsonResult)
        {
            string weatherKey = "319cbb1ff967b87cc559b2e8308d0ddc";
            string weatherUri;
            GeoResult geoResult;

            // get coordinates for location
            try
            {
				string jsonStr = ((string[])jsonResult.Data).First().ToString();
				geoResult = new JavaScriptSerializer().Deserialize<GeoResult>(jsonStr);
				Location coordinates = geoResult.report.results.First().geometry.location;
                weatherUri = String.Format("https://api.darksky.net/forecast/{0}/{1},{2}", weatherKey, coordinates.lat, coordinates.lng);
            }
            catch (Exception ex)
            {
                return this.RedirectToAction("Index", "Could not find coordinates for location.");
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
						WeatherResult weatherResult = new WeatherResult()
						{
							report = JsonConvert.DeserializeObject<WeatherReport>(weatherJson),
							geoResult = null
                        };

						weatherResult.report.daily = null;
						weatherResult.report.hourly = null;

						string jsonObject = new JavaScriptSerializer().Serialize(weatherResult);
						return this.RedirectToAction("Index", Json(jsonObject));
                    }
                }
            }
            catch (Exception ex)
            {

			}
			return this.RedirectToAction("Index", (object)"Could not find weather data.");
		}

		public ActionResult Index(object model)
		{
			return View(model);
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