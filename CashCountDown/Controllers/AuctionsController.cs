using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CashCountDown.Models;
using System.Web.Security;
using Newtonsoft.Json;
using CashCountDown.Interfaces;
using CashCountDown.Repositories;
using CashCountDown.Helpers;
using Microsoft.Owin.Security;
using WebMatrix.WebData;
using System.Net;

namespace CashCountDown.Controllers
{
    public partial class AuctionsController : Controller
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private int _userId = (WebSecurity.IsAuthenticated) ? WebSecurity.CurrentUserId : 0;
       // private int _userId = (AuthenticationManager.User) ? AuthenticationManager.CurrentUserId : 0;
        private AuctionRepository AuctionRepo = new AuctionRepository();
        private AutoBidRepository AutoBidRepo = new AutoBidRepository();
        private readonly AuctionHelpers helper = new AuctionHelpers();

        //
        // GET: /Auctions/

        public virtual ActionResult Index()
        {
            var auctions = AuctionRepo.get();

            return View(auctions);
        }

        //
        // GET: /Auctions/Details/5
        public virtual ActionResult Details(int id = 0)
        {
            Auction_Details _auctionDetails = AuctionRepo.get(id);

            if (_auctionDetails == null)
            {
                return null;
            }

            if ((_auctionDetails.Active == false) & (!Roles.IsUserInRole("Administrator"))) { return View("NotActive"); }



            return View(_auctionDetails);
        }



        //
        // GET: /Auctions/Create
        [Authorize(Roles = "Administrator")]
        public virtual ActionResult Create()
        {
            Auction_Create create = AuctionRepo.getCreate();
            return View(create);
        }

        //
        // POST: /Auctions/Create
        [HttpPost, Authorize(Roles = "Administrator"), ValidateAntiForgeryToken]
        public virtual ActionResult Create(Auction_Create auction)
        {
            if (ModelState.IsValid)
            {
                bool created = AuctionRepo.create(auction);
                if (created)
                {
                    return RedirectToAction("Details", "Auctions", new { id = auction.AuctionId });
                }
                else
                {
                    return View(auction);
                }
            }
            return View(auction);
        }

        //
        // GET: /Auctions/Edit/5
        [Authorize(Roles = "Administrator")]
        public virtual ActionResult Edit(int id = 0)
        {
            Auction_Edit auction = AuctionRepo.getEdit(id);
            if (auction == null)
            {
                return HttpNotFound();
            }

            return View(auction);
        }

        //
        // POST: /Auctions/Edit/5

        [HttpPost, Authorize(Roles = "Administrator"), ValidateAntiForgeryToken]
        public virtual ActionResult Edit(Auction_Edit auction)
        {
            if (ModelState.IsValid)
            {
                bool edit = AuctionRepo.edit(auction);
                if (edit)
                {
                    return RedirectToAction("Details", "Auctions", new { id = auction.AuctionId });
                }
            }
            return View(auction);
        }

        //
        // GET: /Auctions/Deactivate/5
        [Authorize(Roles = "Administrator")]
        public virtual ActionResult Deactivate(int id = 0)
        {
            ToDeactivate auction = AuctionRepo.getDeactivate(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        //
        // POST: /Auctions/Deactivate/5

        [HttpPost, ActionName("Deactivate"), Authorize(Roles = "Administrator"), ValidateAntiForgeryToken]
        public virtual ActionResult DeactivateConfirmed(int id)
        {
            //AuctionRepo.Deactivate(id);
            return RedirectToAction("Index");
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public virtual JsonResult PlaceBidAjax(int id)
        {
            var status = AuctionRepo.placeBid(id);
            return Json(status);
        }


        //Submit via btn using Websecurity userid
        //and 
        //Submit via ghost bidder service
        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public virtual ActionResult SubmitOrder(SubmitOrder OrderContents)
        {
            OrderContents = new SubmitOrder
            {
                UserId = _userId
            };
            //check if empty
            if (OrderContents.BidPackageId < 0 || OrderContents.AuctionId < 0)
            {
                return RedirectToAction("Index", "Auctions");
            }

            //submit order
            bool orderSubmitted = AuctionRepo.submitOrder(OrderContents);
            if (orderSubmitted)
            {
                return RedirectToAction("Index", "Auctions");
            }
            else
            {
                return RedirectToAction("Index", "Auctions");
            }
        }



        public virtual ActionResult addtoList(int id, string list)
        {
            WishList_Auction wish = new WishList_Auction();
            wish.AuctionId = id;
            //wish.ListName = list;
            wish.UserId = _userId;
            return Json(AuctionRepo.addtoList(wish));
        }












        [HttpPost]
        public virtual JsonResult statusupdate(DateTime tt, Array ids)
        {
            return Json(AuctionRepo.statusupdate(tt, ids));
        }


        public virtual JsonResult ourtime(DateTime TheirTime)
        {
            return Json(DateTime.Now);

        }

        #region Autobids
        //
        // POST: /AutoBid/CreaNewAutoBidte

        [HttpPost, Authorize, ValidateAntiForgeryToken]

        public bool NewAutoBid(AutoBid autobid)
        {
            return AutoBidRepo.NewAutoBid(autobid);
        }


        //
        // POST: /AutoBid/EditAutoBid/5

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public virtual bool EditAutoBid(AutoBid autobid)
        {
            return AutoBidRepo.EditAutoBid(autobid);
        }

        //
        // POST: /AutoBid/DeactivateAutoBid/5

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public virtual bool DeactivateAutoBid(int id)
        {
            return AutoBidRepo.DeactivateAutoBid(id);
        }
        #endregion


        //Seach and commit all autobids
        public bool DoAutobids(int id)
        {
            return AutoBidRepo.DoAutobids(id);
        }



    }
}