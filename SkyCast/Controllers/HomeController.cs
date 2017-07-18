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