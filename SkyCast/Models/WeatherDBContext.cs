using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyCast.Models
{
	public class WeatherDbContext : DbContext
	{
		public DbSet<Query> queries { get; set; }
		public DbSet<GeoResult> geoResults { get; set; }
		public DbSet<WeatherResult> weatherResults { get; set; }

		public System.Data.Entity.DbSet<SkyCast.Models.Location> Locations { get; set; }
	}

	public class Query
	{
		public int Id { get; set; }
		public string location { get; set; }
		public string dateTime { get; set; }
		//[ForeignKey("Id")]
		//public GeoResult geoResult { get; set; }
		//[ForeignKey("Id")]
		//public WeatherResult weatherResult { get; set; }
	}

	public class WeatherResult
	{
		public int Id { get; set; }
		public string geoReport { get; set; }
		[ForeignKey("Id")]
		public Query query { get; set; }
	}

	public class GeoResult
	{
		public int Id { get; set; }
		public string geoReport { get; set; }
		[ForeignKey("Id")]
		public Query query { get; set; }
	}

	public class QueryRepository
	{
		public ICollection<Query> GetResults()
		{
			WeatherDbContext weatherDbContext = new WeatherDbContext();
			return weatherDbContext.queries.Include("WeatherResult").Include("GeoResult").ToList();
		}
	}
}