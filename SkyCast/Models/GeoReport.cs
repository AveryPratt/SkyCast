using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyCast.Models
{
    public class AddressComponent
    {
		public int Id { get; set; }
		public string long_name { get; set; }
        public string short_name { get; set; }
        public virtual ICollection<string> types { get; set; }
    }

    public class Northeast
	{
		public int Id { get; set; }
		public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest
	{
		public int Id { get; set; }
		public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Bounds
	{
		public int Id { get; set; }
		public virtual Northeast northeast { get; set; }
        public virtual Southwest southwest { get; set; }
    }

    public class Location
	{
		public int Id { get; set; }
		public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Northeast2
	{
		public int Id { get; set; }
		public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest2
	{
		public int Id { get; set; }
		public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Viewport
	{
		public int Id { get; set; }
		public virtual Northeast2 northeast { get; set; }
        public virtual Southwest2 southwest { get; set; }
    }

    public class Geometry
	{
		public int Id { get; set; }
		public virtual Bounds bounds { get; set; }
		public virtual Location location { get; set; }
        public string location_type { get; set; }
		public virtual Viewport viewport { get; set; }
	}

    public class Result
	{
		public int Id { get; set; }
		public virtual ICollection<AddressComponent> address_components { get; set; }
		public string formatted_address { get; set; }
        public virtual Geometry geometry { get; set; }
        public string place_id { get; set; }
        public virtual ICollection<string> types { get; set; }
    }

    public class GeoReport
	{
		public int Id { get; set; }
		public virtual ICollection<Result> results { get; set; }
        public string status { get; set; }
    }
}