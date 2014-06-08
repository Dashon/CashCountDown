using CashCountDown.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebMatrix.WebData;
using System.Data.Entity;
using CashCountDown.Interfaces;

namespace CashCountDown.Repositories
{
    public class AutoBidRepository : IAutoBidRepository
    {
        private CashCountDownContext db { get; set; }
        private int _userId { get; set; }

        public AutoBidRepository(int _userId = 0)
        {
            db = new CashCountDownContext();
            _userId = WebSecurity.CurrentUserId;
        }

        public bool NewAutoBid(AutoBid autobid)
        {
            Auction auction = db.Auctions.Find(autobid.AuctionId);
            autobid.UserId = _userId;
            autobid.CreatedOn = DateTime.Now;
            autobid.Active = true;
            if (auction.ExpirationDate < DateTime.Now.AddMinutes(1)) { return false; }
            if (autobid.StartAmount < auction.Price) { return false; }
            if (autobid.MaxAmount < autobid.StartAmount) { return false; }
            autobid.RemainingBids = autobid.MaxBids;
            int bal = 0;
            bal = db.UserProfiles.Find(autobid.UserId).BidBalance;
            if (autobid.MaxBids > bal) { return false; }

            if (db.AutoBids.Where(x => x.UserId == _userId
                & x.AuctionId == autobid.AuctionId).Count() > 0)
            { return false; }

            try
            {
                db.AutoBids.Add(autobid);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public virtual bool EditAutoBid(AutoBid autobid)
        {
            AutoBid newautobid = db.AutoBids.Find(autobid.AutoBidId);
            if (newautobid.Auction.ExpirationDate < DateTime.Now.AddMinutes(1)) { return false; }

            if (newautobid.UserId == _userId)
            {
                newautobid.RemainingBids = autobid.MaxBids;
                newautobid.MaxAmount = autobid.MaxAmount;
                newautobid.StartAmount = autobid.StartAmount;
                newautobid.MaxBids = autobid.MaxBids;
                newautobid.ModifiedOn = DateTime.Now;
                try
                {
                    db.Entry(newautobid).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            return false;
        }


        public virtual bool DeactivateAutoBid(int id)
        {
            AutoBid autobid = db.AutoBids.Find(id);
            autobid.Active = false;
            db.Entry(autobid).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }


        //Seach and commit all autobids
        public bool DoAutobids(int id)
        {
            IEnumerable<AutoBid> ABs = db.AutoBids
                .Include("UserProfile")
                .Include("Auction")
                .Include(y => y.Auction.Bids.Select(x => x.UserProfile))
                .OrderBy(x => x.MaxAmount)
                .Where(d => d.AuctionId == id & d.Active == true);

            foreach (AutoBid ab in ABs.Where(x => x.Active == true))
            {
                Bid newBid = new Bid();
                Auction auction = ab.Auction;
                int userbalance = ab.UserProfile.BidBalance;

                if (auction.Price < ab.StartAmount)
                {
                    newBid.Amount = ab.StartAmount;
                }
                else
                {
                    newBid.Amount = auction.Price + 1;
                }

                if ((newBid.Amount > auction.Price)
                    & (newBid.Amount <= ab.MaxAmount)
                    & (userbalance > 0)
                    & (auction.ExpirationDate > DateTime.Now)
                    & (ab.RemainingBids > 0)
                    & (ab.Active)
                    & (ab.Auction.WinningUserId != ab.UserId))
                {
                    newBid.AuctionId = auction.AuctionId;
                    newBid.UserId = ab.UserId;
                    newBid.CreatedOn = DateTime.Now;
                    newBid.AuctionId = newBid.AuctionId;
                    newBid.IsAutoBid = true;
                    ab.RemainingBids -= 1;
                    ab.UserProfile.BidBalance -= 1;
                    db.Bids.Add(newBid);
                    auction.ExpirationDate = auction.ExpirationDate.AddSeconds(30);
                    auction.Price = newBid.Amount;
                    auction.WinningUserId = newBid.UserId;
                }

                if (auction.ExpirationDate < DateTime.Now)
                {
                    DeactivateAutoBid(ab.AutoBidId);
                }

            }

            db.SaveChanges();
            return true;
        }


    }
}