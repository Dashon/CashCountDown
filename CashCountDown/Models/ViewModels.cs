using CashCountDown.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Globalization;
using System.Web.Security;

namespace CashCountDown.Models
{

    public class Auction_Details
    {
        public int AuctionId { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string ProductCat { get; set; }
        public int BidCost { get; set; }
        public int Price { get; set; }
        public int BuyItPrice { get; set; }
        public int RetailPrice { get; set; }
        public int Savings { get; set; }
        public int StartPrice { get; set; }
        public int BidIncrement { get; set; }
        public ICollection<BidHistory> Bids { get; set; }
        public string WinnerUserName { get; set; }
        public bool HasAutobids { get; set; }
        public bool Active { get; set; }
        public AutoBid  UserAutoBid { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public Order Order { get; set; }
        public Bid Bid { get; set; }

    }

    public class Auction_Edit
    {
        public int AuctionId { get; set; }
        public int BidCost { get; set; }
        public int BidIncrement { get; set; }
        public bool Active { get; set; }
        public string ProductName { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

     public class Auction_Create
    {
        public int AuctionId { get; set; }
        public int ProductId { get; set; }
        public IEnumerable<Product> AllProducts { get; set; }
        public int StartPrice { get; set; }
        public int BidCost { get; set; }
        public int Price { get; set; }
        public int BidIncrement { get; set; }
        public bool Active { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class Authorizer
    {
        public int UserId { get; set; }
        public int AuctionId { get; set; }
    }

    public class PayPalReturn
    {
        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }
        public string PaymentId { get; set; }
        public string ReceptId { get; set; }

    }

    public class PayPalSubmit
    {
        [Required]
        public UserProfile userprofile { get; set; }
        public Auction auction { get; set; }
        public BidPackage bidpackage { get; set; }
    }

    public class ToDeactivate 
    {
        public string Name { get; set; }
        public int id { get; set; }
    }
    public class BidHistory
    {
        public DateTime DateTime { get; set; }
        public int Amount { get; set; }
        public string userName { get; set; }
        public bool IsAutoBid { get; set; }
    }
}