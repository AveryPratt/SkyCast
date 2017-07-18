using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SkyCast.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace SkyCast.Controllers
{
    public class WeatherController : Controller
    {
        private WeatherDbContext db = new WeatherDbContext();
		
		// GET: Weather
		public ActionResult Index(string location, string dateTime)
		{
			ViewBag.Controller = "Weather";

			if (!User.Identity.IsAuthenticated)
			{
				return this.RedirectToAction("Index", "Home", new { location = location, dateTime = dateTime });
			}

			if (location != null) // empty string is acceptable
			{
				TempDataDictionary data = TempData;
				TempData["geoReport"] = APIHelper.GetGeoLocation(ref data, location, dateTime);
				TempData["weatherReport"] = APIHelper.GetWeatherForecast(ref data, TempData["geoReport"] as GeoReport);
				TempData = data;

				Query query = new Query()
				{
					location = TempData["location"] as String,
					dateTime = TempData["dateTime"] as String
				};
				GeoResult geoResult = new GeoResult()
				{
					query = query,
					geoJson = TempData["geoJson"] as String
				};
				WeatherResult weatherResult = new WeatherResult()
				{
					query = query,
					weatherJson = TempData["weatherJson"] as String
				};
				Session["query"] = query;
				Session["geoResult"] = geoResult;
				Session["weatherResult"] = weatherResult;
			}

			List<Query> pastQueries = db.queries.ToList();
			TempData["pastQueries"] = pastQueries;
			return View(pastQueries);
		}

        // GET: Weather/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Query query = db.queries.Find(id);

			GeoResult geoResult = db.geoResults.First(g => g.query.Id == query.Id);
			WeatherResult weatherResult = db.weatherResults.First(w => w.query.Id == query.Id);

			string geoJson = geoResult.geoJson;
			string weatherJson = weatherResult.weatherJson;

			TempData["geoReport"] = JsonConvert.DeserializeObject<GeoReport>(geoJson);
			TempData["weatherReport"] = JsonConvert.DeserializeObject<WeatherReport>(weatherJson);

			Session["query"] = query;

			return this.RedirectToAction("Index");
		}

		// POST: Weather/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create() //[Bind(Include = "Id,location")] Query query
		{
			Query query = Session["query"] as Query;
			GeoResult geoResult = Session["geoResult"] as GeoResult;
			WeatherResult weatherResult = Session["weatherResult"] as WeatherResult;
            if (query != null &&
				geoResult != null &&
				weatherResult != null &&
				ModelState.IsValid)
            {
                db.queries.Add(query);
				db.geoResults.Add(geoResult);
				db.weatherResults.Add(weatherResult);
				db.SaveChanges();
				Session.Remove("query");
				Session.Remove("geoResult");
				Session.Remove("weatherResult");
				TempData["errorMessage"] = "Data saved successfully.";
				return this.RedirectToAction("Index", "Weather");
            }
			TempData["errorMessage"] = "Could not save data.";
			return this.RedirectToAction("Index", "Weather");
        }

        // GET: Weather/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Query query = db.queries.Find(id);
            if (query == null)
            {
                return HttpNotFound();
            }
            return View(query);
        }

        // POST: Weather/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Query query = db.queries.Find(id);
            db.queries.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
