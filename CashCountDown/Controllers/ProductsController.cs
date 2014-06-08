using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CashCountDown.Models;
using System.Web.Security;
using WebMatrix.WebData;

namespace CashCountDown.Controllers
{
    [Authorize(Roles = "Administrator")]
    public partial class ProductsController : Controller
    {
        private readonly CashCountDownContext db = new CashCountDownContext();

        //
        // GET: /Products/

        public virtual ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.UserProfile);

            return View(products.ToList());
        }

        //
        // GET: /Products/Details/5

        public virtual ActionResult Details(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Products/Create

        public virtual ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName");
            return View();
        }

        //
        // POST: /Products/Create

        [HttpPost, ValidateAntiForgeryToken]

        public virtual ActionResult Create(Product product)
        {
            product.CreatedOn = DateTime.Now;
            product.UserId = WebSecurity.CurrentUserId;
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", product.UserId);
            return View(product);
        }

        //
        // GET: /Products/Edit/5

        public virtual ActionResult Edit(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", product.UserId);
            return View(product);
        }

        //
        // POST: /Products/Edit/5

        [HttpPost, ValidateAntiForgeryToken]

        public virtual ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", product.UserId);
            return View(product);
        }

        //
        // GET: /Products/Delete/5
        [Authorize]
        public virtual ActionResult Delete(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Products/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public virtual ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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