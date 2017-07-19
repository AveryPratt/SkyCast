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
			string geoKey = Environment.GetEnvironmentVariable("GEOKEY");
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
			string weatherKey = Environment.GetEnvironmentVariable("WEATHERKEY");
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
						//data["chart1"] = setChart1(weatherReport);
						//data["chart2"] = setChart2(weatherReport);
						//data["chart3"] = setChart3(weatherReport);
						//data["chart4"] = setChart4(weatherReport);
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

		private static string setChart1(WeatherReport weatherJson)
		{
			List<string> times = new List<string>();
			List<double> temp = new List<double>();
			foreach (var item in weatherJson.hourly.data)
			{
				times.Add(item.time.ToString());
				temp.Add(item.temperature);
			}
			List<Dataset> datasets = new List<Dataset>();
			datasets.Add(new Dataset()
			{
				label = "Temperature",
				data = temp,
				backgroundColor = new List<string>()
				{
					"#FF8080",
					//"#",
					//"#",
					//"#",
					//"#",
					//"#",
				},
				borderColor = new List<string>()
				{
					"#FF0000",
					//"#",
					//"#",
					//"#",
					//"#",
					//"#",
				},
				borderWidth = 1
			});
			Data data = new Data()
			{
				labels = times,
				datasets = datasets
			};
			List<YAx> yAxes = new List<YAx>();
			yAxes.Add(new YAx()
			{
				ticks = new Ticks()
				{
					beginAtZero = true
				}
			});
			ChartModel model = new ChartModel()
			{
				type = "bar",
				options = new Options()
				{
					scales = new Scales()
					{
						yAxes = yAxes
					}
				},
				data = data
			};
			return JsonConvert.SerializeObject(model);
		}

		private static string setChart2(WeatherReport weatherJson)
		{
			List<string> times = new List<string>();
			List<double> probability = new List<double>();
			List<double> intensity = new List<double>();
			foreach (var item in weatherJson.hourly.data)
			{
				times.Add(item.time.ToString());
				probability.Add(item.precipProbability);
				intensity.Add(item.precipIntensity);
			}
			List<Dataset> datasets = new List<Dataset>();
			datasets.Add(new Dataset()
			{
				label = "Chance of Precipitation",
				data = probability,
				backgroundColor = new List<string>()
				{
					"#FF8080",
					//"#",
					//"#",
					//"#",
					//"#",
					//"#",
				},
				borderColor = new List<string>()
				{
					"#FF0000",
					//"#",
					//"#",
					//"#",
					//"#",
					//"#",
				},
				borderWidth = 1
			});
			datasets.Add(new Dataset()
			{
				label = "Precipitation Amount",
				data = intensity,
				backgroundColor = new List<string>()
				{
					"#8080FF",
					//"#",
					//"#",
					//"#",
					//"#",
					//"#",
				},
				borderColor = new List<string>()
				{
					"#8080FF",
					//"#",
					//"#",
					//"#",
					//"#",
					//"#",
				},
				borderWidth = 1
			});
			Data data = new Data()
			{
				labels = times,
				datasets = datasets
			};
			List<YAx> yAxes = new List<YAx>();
			yAxes.Add(new YAx()
			{
				ticks = new Ticks()
				{
					beginAtZero = true
				}
			});
			ChartModel model = new ChartModel()
			{
				type = "bar",
				options = new Options()
				{
					scales = new Scales()
					{
						yAxes = yAxes
					}
				},
				data = data
			};
			return JsonConvert.SerializeObject(model);
		}

		private static string setChart3(WeatherReport weatherJson)
		{
			List<string> times = new List<string>();
			List<double> maxs = new List<double>();
			List<double> mins = new List<double>();
			foreach (var item in weatherJson.daily.data)
			{
				times.Add(item.time.ToString());
				maxs.Add(item.temperatureMax);
				mins.Add(item.temperatureMin);
			}
			List<Dataset> datasets = new List<Dataset>();
			datasets.Add(new Dataset()
			{
				label = "Max Temperature",
				data = maxs,
				backgroundColor = new List<string>()
				{
					"#FF8080",
					//"#",
					//"#",
					//"#",
					//"#",
					//"#",
				},
				borderColor = new List<string>()
				{
					"#FF0000",
					//"#",
					//"#",
					//"#",
					//"#",
					//"#",
				},
				borderWidth = 1
			});
			datasets.Add(new Dataset()
			{
				label = "Min Temperature",
				data = mins,
				backgroundColor = new List<string>()
				{
					"#8080FF",
					//"#",
					//"#",
					//"#",
					//"#",
					//"#",
				},
				borderColor = new List<string>()
				{
					"#8080FF",
					//"#",
					//"#",
					//"#",
					//"#",
					//"#",
				},
				borderWidth = 1
			});
			Data data = new Data()
			{
				labels = times,
				datasets = datasets
			};
			List<YAx> yAxes = new List<YAx>();
			yAxes.Add(new YAx()
			{
				ticks = new Ticks()
				{
					beginAtZero = true
				}
			});
			ChartModel model = new ChartModel()
			{
				type = "bar",
				options = new Options()
				{
					scales = new Scales()
					{
						yAxes = yAxes
					}
				},
				data = data
			};
			return JsonConvert.SerializeObject(model);
		}

		private static string setChart4(WeatherReport weatherJson)
		{
			List<string> times = new List<string>();
			List<double> probability = new List<double>();
			List<double> intensity = new List<double>();
			foreach (var item in weatherJson.daily.data)
			{
				times.Add(item.time.ToString());
				probability.Add(item.precipProbability);
				intensity.Add(item.precipIntensity);
			}
			List<Dataset> datasets = new List<Dataset>();
			datasets.Add(new Dataset()
			{
				label = "Chance of Precipitation",
				data = probability,
				backgroundColor = new List<string>()
				{
					"#FF8080",
					//"#",
					//"#",
					//"#",
					//"#",
					//"#",
				},
				borderColor = new List<string>()
				{
					"#FF0000",
					//"#",
					//"#",
					//"#",
					//"#",
					//"#",
				},
				borderWidth = 1
			});
			datasets.Add(new Dataset()
			{
				label = "Precipitation Amount",
				data = intensity,
				backgroundColor = new List<string>()
				{
					"#8080FF",
					//"#",
					//"#",
					//"#",
					//"#",
					//"#",
				},
				borderColor = new List<string>()
				{
					"#8080FF",
					//"#",
					//"#",
					//"#",
					//"#",
					//"#",
				},
				borderWidth = 1
			});
			Data data = new Data()
			{
				labels = times,
				datasets = datasets
			};
			List<YAx> yAxes = new List<YAx>();
			yAxes.Add(new YAx()
			{
				ticks = new Ticks()
				{
					beginAtZero = true
				}
			});
			ChartModel model = new ChartModel()
			{
				type = "bar",
				options = new Options()
				{
					scales = new Scales()
					{
						yAxes = yAxes
					}
				},
				data = data
			};
			return JsonConvert.SerializeObject(model);
		}
	}
}