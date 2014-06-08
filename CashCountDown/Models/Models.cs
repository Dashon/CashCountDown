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


    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public int? ShippingAddrId { get; set; }
        [ForeignKey("ShippingAddrId")]
        public virtual ShippingAddr ShippingAddr { get; set; }
        public int? BillingAddrId { get; set; }
        [ForeignKey("BillingAddrId")]
        public virtual BillingAddr BillingAddr { get; set; }
        public ICollection<Bid> Bids { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<AutoBid> AutoBids { get; set; }
        public ICollection<Media> Media { get; set; }
        public ICollection<View> Views { get; set; }
        public int BidBalance { get; set; }
    }
    public class ShippingAddr
    {
        public int ShippingAddrId { get; set; }
        public string Name { get; set; }
        [Display(Name = "Address 1/P.O. Box")]
        public string Addr1 { get; set; }
        [Display(Name = "Address 2")]
        public string Addr2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
    }
    public class BillingAddr
    {
        public int BillingAddrId { get; set; }
        public string Name { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
    }
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public int BuyItPrice { get; set; }
        public int RetailPrice { get; set; }
        public bool Active { get; set; }
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile UserProfile { get; set; }
        public ICollection<Auction> Auctions { get; set; }
    }

    public class Auction : IEntity
    {
        public int AuctionId { get; set; }
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int StartPrice { get; set; }
        public int BidCost { get; set; }
        public int Price { get; set; }
        public int BidIncrement { get; set; }
        public bool Active { get; set; }
        public bool? Sold { get; set; }
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int? WinningUserId { get; set; }
        [ForeignKey("WinningUserId")]
        public virtual UserProfile WinningUser { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile UserProfile { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Bid> Bids { get; set; }
        public ICollection<View> Views { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public String Name { get; set; }
    }

    public class Search
    {
        public int SearchId { get; set; }
        public string Term { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class AutoBid
    {
        public int AutoBidId { get; set; }
        public int AuctionId { get; set; }
        [ForeignKey("AuctionId")]
        public virtual Auction Auction { get; set; }
        public int StartAmount { get; set; }
        public int MaxAmount { get; set; }
        public int MaxBids { get; set; }
        public int RemainingBids { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile UserProfile { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool Active { get; set; }

    }

    public class Media
    {
        public int MediaId { get; set; }
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public string Name { get; set; }
        public string Descriptiopn { get; set; }
        public string Location { get; set; }
        public Object Image { get; set; }
        public Object Thumbnail { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile UserProfile { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class Bid
    {
        public int BidId { get; set; }
        public bool IsAutoBid { get; set; }
        public int Amount { get; set; }
        public int AuctionId { get; set; }
        [ForeignKey("AuctionId")]
        public virtual Auction Auction { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile UserProfile { get; set; }
        [Display(Name = "Date")]
        public DateTime CreatedOn { get; set; }
    }

    public class Gift
    {
        public int GiftId { get; set; }
        public int AuctionId { get; set; }
        [ForeignKey("AuctionId")]
        public virtual Auction Auction { get; set; }
        public int rs { get; set; }
        public DateTime OfferDate { get; set; }
        public DateTime SaleDate { get; set; }
    }

    public class WishList_Auction
    {
        public int WishList_AuctionId { get; set; }
        public int UserId { get; set; }
        // public string ListName { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile UserProfile { get; set; }
        public int AuctionId { get; set; }
        [ForeignKey("AuctionId")]
        public virtual Auction Auction { get; set; }

    }
    public class Shipment
    {
        public int ShipmentId { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        public int TrackingNumber { get; set; }
        public int Status { get; set; }
        public DateTime ShipDate { get; set; }
    }


    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile UserProfile { get; set; }
        public int? AuctionId { get; set; }
        [ForeignKey("AuctionId")]
        public virtual Auction Auction { get; set; }
        public int? BidPackageId { get; set; }
        public int Amount { get; set; }
        public int TransactionId { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }

    }

    public class BidPackage
    {
        public int BidPackageId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Cost { get; set; }
        public bool Active { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
    public class View
    {
        public int ViewId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile UserProfile { get; set; }
        public int AuctionId { get; set; }
        [ForeignKey("AuctionId")]
        public virtual Auction Auction { get; set; }
        public DateTime ViewDate { get; set; }
    }

}