using Newtonsoft.Json;
using SkyCast.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SkyCast.Controllers
{
	public class APIHelper
	{
		public static GeoReport GetGeoLocation(ref TempDataDictionary data, string location, string dateTime)
		{
			data["location"] = location;
			try
			{
				data["dateTime"] = String.IsNullOrEmpty(dateTime) ? null : (DateTime?)Convert.ToDateTime(dateTime);
			}
			catch (Exception ex)
			{
				data["errorMessage"] = "Unrecognizable date format.";
				return null;
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
						data["geoJson"] = geoJson;
						GeoReport geoReport = JsonConvert.DeserializeObject<GeoReport>(geoJson);
						return geoReport;
					}
					throw new HttpRequestException("Bad Request.");
				}
			}
			catch (HttpRequestException)
			{
				data["errorMessage"] = "Could not find location.";
			}
			return null;
		}

		public static WeatherReport GetWeatherForecast(ref TempDataDictionary data, GeoReport geoReport)
		{
			string weatherKey = "319cbb1ff967b87cc559b2e8308d0ddc";
			string weatherUri;
			DateTime? dateTime = (DateTime?)data["datetime"];

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
				data["errorMessage"] = "Could not find coordinates for location.";
				return null;
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
						data["weatherJson"] = weatherJson;
						WeatherReport weatherReport = JsonConvert.DeserializeObject<WeatherReport>(weatherJson);
						return weatherReport;
					}
					else
					{
						throw new HttpRequestException("Bad Request.");
					}
				}
			}
			catch (HttpRequestException)
			{
				data["errorMessage"] = "Could not find weather data.";
			}
			return null;
		}
	}
}