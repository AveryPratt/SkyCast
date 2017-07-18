using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SkyCast.Models;

namespace SkyCast.Controllers
{
    public class WeatherController : Controller
    {
        private WeatherDbContext db = new WeatherDbContext();

        // GET: Weather
        public ActionResult Index()
        {
            return View(db.queries.ToList());
        }

        // GET: Weather/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Query query = db.queries.Find(id);

			IQueryable<Query> qu = from q in db.queries
					 join wr in db.weatherReports on q.weatherReport equals wr
					 join gr in db.geoReports on q.geoReport equals gr
					 select new Query()
					 {
						 weatherReport = wr,
						 geoReport = gr
					 };
            if (query == null)
            {
                return HttpNotFound();
			}
			return this.RedirectToAction("Index", "Home", );
		}

		//// GET: Weather/Create
		//public ActionResult Create()
		//{
		//    return View();
		//}

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
				db.weatherReports.Add(query.weatherReport);
				db.currently.Add(query.weatherReport.currently);
				db.hourly.Add(query.weatherReport.hourly);
				db.datum.AddRange(query.weatherReport.hourly.data);
				db.daily.Add(query.weatherReport.daily);
				db.datum2.AddRange(query.weatherReport.daily.data);
				db.flags.Add(query.weatherReport.flags);
				db.geoReports.Add(query.geoReport);
				db.results.AddRange(query.geoReport.results);
				foreach (var result in query.geoReport.results)
				{
					db.addressComponents.AddRange(result.address_components);
					db.geometry.Add(result.geometry);
					db.bounds.Add(result.geometry.bounds);
					db.northeast.Add(result.geometry.bounds.northeast);
					db.southwest.Add(result.geometry.bounds.southwest);
					db.locations.Add(result.geometry.location);
					db.viewports.Add(result.geometry.viewport);
					db.northeast2.Add(result.geometry.viewport.northeast);
					db.southwest2.Add(result.geometry.viewport.southwest);
				}
				db.SaveChangesAsync();
				Session.Remove("query");
				TempData["errorMessage"] = "Data saved successfully.";
				return this.RedirectToAction("Index", "Home");
            }
			TempData["errorMessage"] = "Could not save data.";
			return this.RedirectToAction("Index", "Home");
        }

        // GET: Weather/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Weather/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,location")] Query query)
        {
            if (ModelState.IsValid)
            {
                db.Entry(query).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(query);
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
