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
			if (!User.Identity.IsAuthenticated)
			{
				return this.RedirectToAction("Index", "Home", new { location = location, dateTime = dateTime });
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
			return this.RedirectToAction("Index", "Home");
		}

		// POST: Weather/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create() //[Bind(Include = "Id,location")] Query query
		{
			Query query = Session["query"] as Query;
            if (query != null && ModelState.IsValid)
            {
                db.queries.Add(query);
				db.SaveChanges();
				Session.Remove("query");
				TempData["errorMessage"] = "Data saved successfully.";
				return this.RedirectToAction("Index", "Home");
            }
			TempData["errorMessage"] = "Could not save data.";
			return this.RedirectToAction("Index", "Home");
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
