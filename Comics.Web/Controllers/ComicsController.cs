using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Comics.Lib;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Comics.Web.Controllers
{
    public class ComicsController : Controller
    {
        private ComicContext db = new ComicContext();

        // GET: Comics
        public ActionResult Index()
        {
            return View(db.Comics.ToList());
        }

        // GET: Comics Search
        public ViewResult Search(string searchName)
        {
            var comics = from s in db.Comics
                           select s;
            if (!String.IsNullOrEmpty(searchName))
            {
                comics= comics.Where(s => s.Name.Contains(searchName) || s.Publisher.Contains(searchName));
            }
            return View("Index", comics.ToList());
        }

        // GET: Comics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comic comic = db.Comics.Find(id);
            if (comic == null)
            {
                return HttpNotFound();
            }
            return View(comic);
        }

        // GET: Comics/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Marvel(string searchName, string searchIssue)
        {
            List<Comic> comics=null;
            try
            {
                //if (!String.IsNullOrEmpty(searchName) || !String.IsNullOrEmpty(searchIssue))
                //{
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = client.GetAsync(Comics.Lib.Marvel.GetMarvelHash(searchName, searchIssue)).Result;

                        response.EnsureSuccessStatusCode();
                        comics = new List<Comic>();
                        string retVal = response.Content.ReadAsStringAsync().Result;

                        dynamic resultado = JsonConvert.DeserializeObject(retVal);
                        foreach (var res in resultado.data.results)
                        {
                            Comic comic = new Comic();
                            comic.ID = res.id;
                            comic.Name = res.title;
                            comic.Issue = res.issueNumber;
                            comic.Price = res.prices[0].price;
                            comic.Thumbnail = res.thumbnail.path + "." + res.thumbnail.extension;
                            comics.Add(comic);
                        }
                    }
                //}
            }
            catch (Exception e)
            {
                var retVal = e.Message;
                return null;
            }
            return View("Marvel",comics);
        }

        // POST: Comics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Issue,Publisher,Price")] Comic comic)
        {
            if (ModelState.IsValid)
            {
                db.Comics.Add(comic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(comic);
        }

        // GET: Comics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comic comic = db.Comics.Find(id);
            if (comic == null)
            {
                return HttpNotFound();
            }
            return View(comic);
        }

        // POST: Comics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Issue,Publisher,Price")] Comic comic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comic);
        }

        // GET: Comics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comic comic = db.Comics.Find(id);
            if (comic == null)
            {
                return HttpNotFound();
            }
            return View(comic);
        }

        // POST: Comics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comic comic = db.Comics.Find(id);
            db.Comics.Remove(comic);
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
