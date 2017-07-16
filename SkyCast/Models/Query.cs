using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyCast.Models
{
	public class Query
	{
		public string location { get; set; }
		public WeatherReport weatherReport { get; set; }
		public GeoReport geoReport { get; set; }
	}
}