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

			if (User.Identity.IsAuthenticated)
			{
				return this.RedirectToAction("Index", "Weather", new { location = location, dateTime = dateTime });
			}
			
			if (!String.IsNullOrEmpty(location))
			{
				TempData = APIHelper.GetGeoLocation(TempData, location, dateTime);
				TempData = APIHelper.GetWeatherForecast(TempData);
			}

			Query query = new Query()
			{
				location = TempData["location"] as String,
			};
			Session["query"] = query;

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