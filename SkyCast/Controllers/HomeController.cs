using SkyCast.Models;
using System;
using System.Web.Mvc;

namespace SkyCast.Controllers
{
	public class HomeController : Controller
    {

		public ActionResult Index(string location, string dateTime)
		{
			ViewBag.Message = "Your home page.";
			ViewBag.Controller = "Home";

			if (User.Identity.IsAuthenticated)
			{
				return this.RedirectToAction("Index", "Weather", new { location = location, dateTime = dateTime });
			}

			if (location != null) // empty string is acceptable
			{
				TempDataDictionary data = TempData;
				TempData["geoReport"] = APIHelper.GetGeoLocation(ref data, location, dateTime);
				TempData["weatherReport"] = APIHelper.GetWeatherForecast(ref data, TempData["geoReport"] as GeoReport);
				TempData = data;
			}

			return View();
		}

		public ActionResult About()
        {
            ViewBag.Message = "SkyCast is a web service that provides you with everything you need to know about the weather. Whether you are planning a weekend fishing trip, or observing the effects of global warming over time, SkyCast is the right tool for the job.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact me at apratt91@gmail.com";

            return View();
        }
    }
}