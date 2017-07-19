using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyCast.Models
{
	public class Dataset
	{
		public string label { get; set; }
		public List<double> data { get; set; }
		public List<string> backgroundColor { get; set; }
		public List<string> borderColor { get; set; }
		public int borderWidth { get; set; }
	}

	public class Data
	{
		public List<string> labels { get; set; }
		public List<Dataset> datasets { get; set; }
	}

	public class Ticks
	{
		public bool beginAtZero { get; set; }
	}

	public class YAx
	{
		public Ticks ticks { get; set; }
	}

	public class Scales
	{
		public List<YAx> yAxes { get; set; }
	}

	public class Options
	{
		public Scales scales { get; set; }
	}

	public class ChartModel
	{
		public string type { get; set; }
		public Data data { get; set; }
		public Options options { get; set; }
	}
}