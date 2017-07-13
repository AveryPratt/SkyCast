using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SkyCast.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult CallAPI()
        {
            var lat = 0;
            var lon = 0;
            string key = "319cbb1ff967b87cc559b2e8308d0ddc";
            string uri = String.Format("https://api.darksky.net/forecast/{0}/{1},{2}", key, lat, lon);
            HttpClient client = new HttpClient();
            var response = client.GetAsync(uri).Result;
            return this.View();
        }

        public ActionResult Index()
        {
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