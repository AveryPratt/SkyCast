using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyCast.Models
{
    public class Currently
	{
		public int Id { get; set; }
		public int time { get; set; }
        public string summary { get; set; }
        public string icon { get; set; }
        public double precipIntensity { get; set; }
        public double precipProbability { get; set; }
        public double temperature { get; set; }
        public double apparentTemperature { get; set; }
        public double dewPoint { get; set; }
        public double humidity { get; set; }
        public double windSpeed { get; set; }
        public double windGust { get; set; }
        public int windBearing { get; set; }
        public double cloudCover { get; set; }
        public double pressure { get; set; }
        public double ozone { get; set; }
        public int uvIndex { get; set; }
    }

    public class Datum
	{
		public int Id { get; set; }
		public int time { get; set; }
        public string summary { get; set; }
        public string icon { get; set; }
        public double precipIntensity { get; set; }
        public double precipProbability { get; set; }
        public double temperature { get; set; }
        public double apparentTemperature { get; set; }
        public double dewPoint { get; set; }
        public double humidity { get; set; }
        public double windSpeed { get; set; }
        public double windGust { get; set; }
        public int windBearing { get; set; }
        public double cloudCover { get; set; }
        public double pressure { get; set; }
        public double ozone { get; set; }
        public int uvIndex { get; set; }
    }

    public class Hourly
	{
		public int Id { get; set; }
		public string summary { get; set; }
        public string icon { get; set; }
        public virtual ICollection<Datum> data { get; set; }
    }

    public class Datum2
	{
		public int Id { get; set; }
		public int time { get; set; }
        public string summary { get; set; }
        public string icon { get; set; }
        public int sunriseTime { get; set; }
        public int sunsetTime { get; set; }
        public double moonPhase { get; set; }
        public double precipIntensity { get; set; }
        public double precipIntensityMax { get; set; }
        public int precipIntensityMaxTime { get; set; }
        public double precipProbability { get; set; }
        public string precipType { get; set; }
        public double temperatureMin { get; set; }
        public int temperatureMinTime { get; set; }
        public double temperatureMax { get; set; }
        public int temperatureMaxTime { get; set; }
        public double apparentTemperatureMin { get; set; }
        public int apparentTemperatureMinTime { get; set; }
        public double apparentTemperatureMax { get; set; }
        public int apparentTemperatureMaxTime { get; set; }
        public double dewPoint { get; set; }
        public double humidity { get; set; }
        public double windSpeed { get; set; }
        public double windGust { get; set; }
        public int windGustTime { get; set; }
        public int windBearing { get; set; }
        public double cloudCover { get; set; }
        public double pressure { get; set; }
        public double ozone { get; set; }
        public int uvIndex { get; set; }
        public int uvIndexTime { get; set; }
    }

    public class Daily
	{
		public int Id { get; set; }
		public string summary { get; set; }
        public string icon { get; set; }
        public virtual ICollection<Datum2> data { get; set; }
    }

    public class Flags
	{
		public int Id { get; set; }
		public ICollection<string> sources { get; set; }
        public string units { get; set; }
    }

    public class WeatherReport
	{
		public int Id { get; set; }
		public double latitude { get; set; }
        public double longitude { get; set; }
        public string timezone { get; set; }
        public double offset { get; set; }
        public virtual Currently currently { get; set; }
		public virtual Hourly hourly { get; set; }
		public virtual Daily daily { get; set; }
		public virtual Flags flags { get; set; }
    }
}