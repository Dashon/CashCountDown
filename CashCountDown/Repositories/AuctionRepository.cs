using CashCountDown.Helpers;
using CashCountDown.Interfaces;
using CashCountDown.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace CashCountDown.Repositories
{

    public class AuctionRepository : IAuctionRepository
    {
        private CashCountDownContext db { get; set; }
        private int _userId { get; set; }

        public AuctionRepository()
        {
            db = new CashCountDownContext();
            _userId = WebSecurity.CurrentUserId;
        }

        public IQueryable<Auction_Details> get()
        {
            var maxTime = DateTime.Now.AddHours(3);
            var auctions = db.Auctions
                .Include("Product")
                .Include("WinningUser")
                .Where(x => x.Active == true
                    & x.ExpirationDate > DateTime.Now);
                    //   & x.ExpirationDate < maxTime)
                    
            ICollection<Auction_Details> ads = null;
            foreach (Auction auction in auctions)
            {
                ads.Add(auctionToAuctionDetails(auction, _userId));
            }
            return ads.AsQueryable();
        }



        public Auction_Details get(int id)
        {
            Auction_Details _auctionDetails = new Auction_Details();
            Auction auction = db.Auctions
                .Include("Bids")
                .Include("WinningUser")
                .Single(y => y.AuctionId == id);
            return auctionToAuctionDetails(auction, _userId);
        }

        public string placeBid(int id, int userId)
        {
            var MainRepo = new MainRepository<Auction>(db);
            Auction auction = db.Auctions
               .Where(x => x.AuctionId == id)
               .First();

            var Amount = auction.Price + auction.BidIncrement;

            if (auction.WinningUserId == userId)
            {
                return "You are already winning";
            }

            Bid bid = new Bid
            {
                Amount = Amount
                ,
                CreatedOn = DateTime.Now
                ,
                AuctionId = id
                ,
                UserId = userId
                ,
                IsAutoBid = false
            };

            if ((getBidBalance(userId) >= 1)
                & (Amount > auction.Price)
                & (auction.ExpirationDate > DateTime.Now)
                & (auction.Active == true))
            {
                db.UserProfiles.Find(userId).BidBalance -= 1;
                db.Bids.Add(bid);
                auction.Price = bid.Amount;
                auction.ExpirationDate = auction.ExpirationDate.AddSeconds(30);
                auction.WinningUserId = bid.UserId;
                db.SaveChanges();
                return "Bid Placed";
            }
            return "Something went Wrong";

        }

        public Auction_Edit getEdit(int id)
        {
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            { return null; }

            return new Auction_Edit
            {
                Active = auction.Active,
                AuctionId = auction.AuctionId,
                BidCost = auction.BidCost,
                BidIncrement = auction.BidIncrement,
                ExpirationDate = auction.ExpirationDate,
                ProductName = auction.Product.Name
            };

        }

        public bool edit(Auction_Edit changes)
        {
            Auction auction = db.Auctions.Find(changes.AuctionId);
            if (changes == null || auction == null)
            { return false; }
            auction.ModifiedOn = DateTime.Now;
            auction.ExpirationDate = changes.ExpirationDate;
            auction.Active = changes.Active;
            auction.BidCost = changes.BidCost;
            auction.BidIncrement = changes.BidIncrement;
            db.Entry(auction).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        public Auction_Create getCreate()
        {
            Auction_Create create = new Auction_Create
            {
                Active = true,
                BidCost = 1,
                BidIncrement = 1,
                ExpirationDate = DateTime.Now.AddDays(1),
                AllProducts = db.Products.Where(x => x.Active == true)
            };
            return create;
        }

        public int create(Auction_Create create, int userId)
        {
            if (create == null) { return 0; }
            Auction auction = new Auction
            {
                Active = create.Active,
                BidCost = create.BidCost,
                BidIncrement = create.BidIncrement,
                CreatedOn = DateTime.Now,
                ExpirationDate = create.ExpirationDate,
                Price = create.Price,
                ProductId = create.ProductId,
                StartPrice = create.StartPrice,
                UserId = userId
            };
            db.Auctions.Add(auction);
            db.SaveChanges();
            return auction.AuctionId;

        }

        public ToDeactivate getDeactivate(int id)
        {

            Auction auction = db.Auctions.Include("Product")
                .First(x => x.AuctionId == id);
            if (auction == null) { return null; }

            return new ToDeactivate
            {
                Name = auction.Product.Name,
                id = auction.AuctionId
            };

        }



        public bool submitOrder(SubmitOrder OrderContents, int userId)
        {
            Boolean PayPalSuccess = false;
            Order order = new Order
            {
                OrderDate = DateTime.Now,
                Status = "Paid"
            };



            UserProfile usr = db.UserProfiles.Find(userId);

            if (OrderContents.BidPackageId > 0)
            {
                BidPackage bp = db.BidPackages.Find(OrderContents.BidPackageId);
                PayPalSubmit PayPalSubmitContents = new PayPalSubmit
                {
                    bidpackage = bp
                    ,
                    userprofile = usr
                };
                PayPalSuccess = submitPayPal(PayPalSubmitContents, _userId);
                order.BidPackageId = OrderContents.BidPackageId;
                order.Amount = bp.Amount;
            }
            else if (OrderContents.AuctionId > 0)
            {
                Auction auction = db.Auctions.Find(OrderContents.AuctionId);
                PayPalSubmit PayPalSubmitContents = new PayPalSubmit
                {
                    auction = auction
                    ,
                    userprofile = usr
                };
                if (auction.Active == true)
                {
                    PayPalSuccess = submitPayPal(PayPalSubmitContents, _userId);
                    order.AuctionId = OrderContents.AuctionId;
                    order.Amount = auction.Product.BuyItPrice;
                }
            }
            else
            {
                return false;
            }

            if (PayPalSuccess)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                if (OrderContents.BidPackageId > 0)
                {
                    LoadBidPackage bidpackageContent = new LoadBidPackage
                    {
                        UserId = userId
                        ,
                        PackageId = OrderContents.BidPackageId
                    };
                    loadBidPackage(bidpackageContent);
                }
            }
            return true;
        }

        public string addtoList(WishList_Auction wish)
        {
            try
            {
                db.WishList_Auctions.Add(wish);
                db.SaveChanges();
                return "Added";
            }
            catch
            {
                return "Failed";
            }

        }

        //  public JsonResult statusUpdate(DateTime tt, String h = "no");

        public Boolean markShipped(Shipment Shipment)
        {
            db.Orders.Find(Shipment.OrderId).Status = "Shipped";
            try
            {
                db.Shipments.Add(Shipment);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean updateShipment(Shipment Shipment)
        {
            try
            {
                db.Shipments.Attach(Shipment);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean addMedia(Media newMedia)
        {
            try
            {
                db.Media.Add(newMedia);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean removeMedia(int MediaId)
        {
            try
            {
                db.Media.Remove(db.Media.Find(MediaId));
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int gGetItemCost(int AuctionId)
        {
            int cost;
            cost = db.Auctions.Find(AuctionId).Price;
            return cost;
        }

        public UserProfile winningBidder(int AuctionId)
        {
            UserProfile winner = new UserProfile();
            Auction auction = db.Auctions
                .Include("Bids")
                .Where(x => x.AuctionId == AuctionId)
                .FirstOrDefault();
            if (auction != null & auction.Bids.Count() > 0)
            {
                winner = auction.Bids
                        .OrderByDescending(x => x.CreatedOn)
                        .First()
                        .UserProfile;
            }
            return winner;
        }

        public IQueryable<BidHistory> BidHistory(int AuctionId)
        {
            ICollection<BidHistory> bids = null;
            IEnumerable<Bid> bidFull = db.Bids.Where(b => b.AuctionId == AuctionId);
            foreach (Bid b in bidFull)
            {
                BidHistory bhx = new BidHistory
                {
                    Amount = b.Amount,
                    DateTime = b.CreatedOn,
                    IsAutoBid = b.IsAutoBid,
                    userName = b.UserProfile.UserName
                };
                bids.Add(bhx);
            }
            return bids.AsQueryable();
        }

        //users
        public int getBidBalance(int Userid)
        {
            int bal = 0;
            bal = db.UserProfiles.Find(Userid).BidBalance;
            return bal;
        }
        public Boolean loadBidPackage(LoadBidPackage bidpackageContents)
        {
            try
            {
                db.UserProfiles.Find(bidpackageContents.UserId).BidBalance
                += db.BidPackages.Find(bidpackageContents.PackageId).Amount;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean submitPayPal(PayPalSubmit PayPalSubmitContents, int userId)
        {
            int amount = 0;
            if (PayPalSubmitContents.bidpackage != null)
            {
                amount = PayPalSubmitContents.bidpackage.Cost;

            }
            else if (PayPalSubmitContents.auction != null)
            {
                if ((PayPalSubmitContents.auction.ExpirationDate < DateTime.Now)
                    & (PayPalSubmitContents.auction.WinningUserId == userId))
                {
                    amount = PayPalSubmitContents.auction.Price;
                }
                else
                {
                    amount = PayPalSubmitContents.auction.Product.BuyItPrice;
                }
            }

            //Enter PayPal Command
            return true;
        }

        private Auction_Details auctionToAuctionDetails(Auction auction, int userId)
        {
            Auction_Details _auctionDetails = new Auction_Details();

            if (auction == null)
            {
                return null;
            }
            //build new autbid
            AutoBid newautobid = new AutoBid();
            newautobid.AuctionId = auction.AuctionId;
            newautobid.Auction = auction;


            _auctionDetails.UserAutoBid = newautobid;
            _auctionDetails.HasAutobids = false;
            _auctionDetails.WinnerUserName = "No Bids";
            _auctionDetails.ProductName = auction.Product.Name;
            _auctionDetails.Price = auction.Price;
            _auctionDetails.Bids = auction.Bids.Select(x => new BidHistory()
            {
                DateTime = x.CreatedOn,
                Amount = x.Amount,
                userName = x.UserProfile.UserName,
                IsAutoBid = x.IsAutoBid
            }).ToList();

            if (auction.WinningUser != null)
            {
                _auctionDetails.WinnerUserName = auction.WinningUser.UserName;
            }

            //check if user has autobids for this item
            if (userId != 0)
            {
                var MyAutobid = db.AutoBids
                    .Where(x => x.AuctionId == auction.AuctionId & x.Active == true & x.UserId == userId)
                    .FirstOrDefault();
                if (MyAutobid != null)
                {
                    _auctionDetails.HasAutobids = true;
                    _auctionDetails.UserAutoBid = MyAutobid;
                }
            }

            Bid bd = new Bid
            {
                AuctionId = auction.AuctionId
            ,
                Auction = auction
            };
            Order ord = new Order
            {
                AuctionId = auction.AuctionId
              ,
                Auction = auction
            };

            _auctionDetails.Order = ord;
            _auctionDetails.Bid = bd;

            var retaiPrice = auction.Product.RetailPrice;
            _auctionDetails.RetailPrice = retaiPrice;
            _auctionDetails.Savings = retaiPrice - auction.Price;
            _auctionDetails.ProductDesc = auction.Product.Description;
            _auctionDetails.ProductCat = auction.Product.Category.Name;
            _auctionDetails.StartPrice = auction.StartPrice;
            _auctionDetails.Active = auction.Active;
            _auctionDetails.AuctionId = auction.AuctionId;
            return _auctionDetails;

        }


        public virtual IQueryable<UpdateResult> statusupdate(DateTime tt, Array ids, int userId)
        {
            ICollection<UpdateResult> AuctionUpdates = new List<UpdateResult>();
            var bidbalance = 0;
            if (userId != 0)
            {
                bidbalance = getBidBalance(userId);
            }
            var Auctions = db.Auctions.Include("WinningUser").Include("Product").Where(x => x.ExpirationDate > tt
                & x.Active == true);

            foreach (int id in ids)
            {
                Auction auction = db.Auctions.Include("WinningUser").Include("Product").First(x => x.AuctionId == id);
                UpdateResult UR = new UpdateResult();
                UR.serverTimeString = DateTime.Now;
                UR.Auction = "auction_" + auction.ProductId;
                UR.Balance = bidbalance;
                UR.Price = auction.Price;
                UR.WinningBidder = auction.WinningUser.UserName;
                UR.price_increment = auction.BidIncrement;
                UR.CanBid = (auction.ExpirationDate > DateTime.Now.AddMinutes(3)) ? true : false;
                UR.TimeLeft = auction.ExpirationDate.Subtract(DateTime.Now);
                UR.savingsPercentage = (auction.Price / auction.Product.RetailPrice) * 100;
                UR.savingsPrice = (auction.Product.RetailPrice - auction.Price);
                // UR.BidHistory = auction.Bids.ToList();
                UR.future = (auction.ExpirationDate > DateTime.Now) ? true : false;
                AuctionUpdates.Add(UR);
            }
            return AuctionUpdates.AsQueryable();

        }
    }
}