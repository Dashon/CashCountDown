using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CashCountDown.Models;

namespace CashCountDown.Controllers
{
    public partial class BidPackagesController : Controller
    {
        private readonly CashCountDownContext db = new CashCountDownContext();

        //
        // GET: /BidPackages/

        public virtual ActionResult Index()
        {
            return View(db.BidPackages.ToList());
        }

        //
        // GET: /BidPackages/Details/5

        public virtual ActionResult Details(int id = 0)
        {
            BidPackage bidpackage = db.BidPackages.Find(id);
            if (bidpackage == null)
            {
                return HttpNotFound();
            }
            return View(bidpackage);
        }

        //
        // GET: /BidPackages/Create

        public virtual ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BidPackages/Create

        [HttpPost]
        public virtual ActionResult Create(BidPackage bidpackage)
        {
            if (ModelState.IsValid)
            {
                db.BidPackages.Add(bidpackage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bidpackage);
        }

        //
        // GET: /BidPackages/Edit/5

        public virtual ActionResult Edit(int id = 0)
        {
            BidPackage bidpackage = db.BidPackages.Find(id);
            if (bidpackage == null)
            {
                return HttpNotFound();
            }
            return View(bidpackage);
        }

        //
        // POST: /BidPackages/Edit/5

        [HttpPost]
        public virtual ActionResult Edit(BidPackage bidpackage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bidpackage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bidpackage);
        }

        //
        // GET: /BidPackages/Delete/5

        public virtual ActionResult Delete(int id = 0)
        {
            BidPackage bidpackage = db.BidPackages.Find(id);
            if (bidpackage == null)
            {
                return HttpNotFound();
            }
            return View(bidpackage);
        }

        //
        // POST: /BidPackages/Delete/5

        [HttpPost, ActionName("Delete")]
        public virtual ActionResult DeleteConfirmed(int id)
        {
            BidPackage bidpackage = db.BidPackages.Find(id);
            db.BidPackages.Remove(bidpackage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {           
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}