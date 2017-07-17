using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SkyCast.Models
{
	public class WeatherDbContext : DbContext
	{
		public DbSet<Query> queries { get; set; }

		public DbSet<WeatherReport> weatherReports { get; set; }
		public DbSet<Currently> currently { get; set; }
		public DbSet<Hourly> hourly { get; set; }
		public DbSet<Daily> daily { get; set; }
		public DbSet<Flags> flags { get; set; }
		public DbSet<Datum> datum { get; set; }
		public DbSet<Datum2> datum2 { get; set; }
		
		public DbSet<GeoReport> geoReports { get; set; }
		public DbSet<Result> results { get; set; }
		public DbSet<AddressComponent> addressComponents { get; set; }
		public DbSet<Geometry> geometry { get; set; }
		public DbSet<Bounds> bounds { get; set; }
		public DbSet<Location> locations { get; set; }
		public DbSet<Viewport> viewports { get; set; }
		public DbSet<Southwest> southwest { get; set; }
		public DbSet<Northeast> northeast { get; set; }
		public DbSet<Southwest2> southwest2 { get; set; }
		public DbSet<Northeast2> northeast2 { get; set; }
	}

	public class Query
	{
		public int Id { get; set; }
		public string location { get; set; }
		public virtual WeatherReport weatherReport { get; set; }
		public virtual GeoReport geoReport { get; set; }
	}

	public class WeatherRepository
	{
		public ICollection<Query> GetResults()
		{
			WeatherDbContext weatherDbContext = new WeatherDbContext();
			return weatherDbContext.queries.ToList();
		}
	}
}