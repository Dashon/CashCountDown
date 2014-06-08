using CashCountDown.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Globalization;
using System.Web.Security;
using CashCountDown.Models;

namespace CashCountDown
{
    public class CashCountDownContext : DbContext, IDbContext
    {
        public CashCountDownContext()
            : base("DefaultConnection")
        {
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
        
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Search> Searchs { get; set; }
        public DbSet<AutoBid> AutoBids { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<BidPackage> BidPackages { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<WishList_Auction> WishList_Auctions { get; set; }
        public DbSet<BillingAddr> BillingAddrs { get; set; }
        public DbSet<ShippingAddr> ShippingAddrs { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

    }

}
