using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASP.NET_MVC_Application.Models;

namespace ASP.NET_MVC_Application.Controllers
{
    public class TechStacksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TechStacks
        public ActionResult Index()
        {
            return View(db.TechStacks.ToList());
        }

        // GET: TechStacks/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechStack techStack = db.TechStacks.Find(id);
            if (techStack == null)
            {
                return HttpNotFound();
            }
            return View(techStack);
        }

        // GET: TechStacks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TechStacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TechStack tech)
        {
            if (ModelState.IsValid)
            {
                var techStack = new TechStack
                {
                    Id = Guid.NewGuid(),
                    Name = tech.Name,
                };

                db.TechStacks.Add(techStack);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tech);
        }

        // GET: TechStacks/Edit/5
        public ActionResult Edit(Guid? id)
        {
            var techStack = db.TechStacks.Find(id);
            if (techStack == null)
            {
                return HttpNotFound();
            }

            var tech = new TechStack
            {
                Id = techStack.Id,
                Name = techStack.Name,
            };

            return View(tech);
        }

        // POST: TechStacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TechStack tech)
        {
            if (ModelState.IsValid)
            {
                var techStack = db.TechStacks.Find(tech.Id);
                if (techStack == null)
                {
                    return HttpNotFound();
                }

                techStack.Name = tech.Name;

                db.Entry(techStack).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tech);
        }

        // GET: TechStacks/Delete/5
        public ActionResult Delete(Guid? id)
        {
            var techStack = db.TechStacks.Find(id);
            if (techStack == null)
            {
                return HttpNotFound();
            }

            return View(techStack);
        }

        // POST: TechStacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var techStack = db.TechStacks.Find(id);
            db.TechStacks.Remove(techStack);
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
