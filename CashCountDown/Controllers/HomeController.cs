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
    public partial class HomeController : Controller
    {
        private readonly CashCountDownContext db = new CashCountDownContext();
        public virtual ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            ViewBag.List1 = db.Auctions.Include(i => i.Product).Include("WinningUser");

            ViewBag.List2 = db.Auctions.Include(i => i.Product).Include("WinningUser");
            return View();
        }

        public virtual ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public virtual ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}
