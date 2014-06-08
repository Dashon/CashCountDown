namespace CashCountDown.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CashCountDown.Models;
    using System.Collections.Generic;



    internal sealed class Configuration : DbMigrationsConfiguration<CashCountDownContext>
    {
        // private readonly CashCountDownContext db = new CashCountDownContext();

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CashCountDownContext context)
        {
            var categories = new List<Category>{

                new Category {Name="Electronics"},
                new Category {Name="Appliances"},
                new Category {Name="Books"}
            };
            categories.ForEach(s => context.Categories.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();


            var products = new List<Product> {
        new Product{Name="DVD Player", CategoryId =1,BuyItPrice = 100, RetailPrice = 90, UserId=1,Active=true,Description="Portable DVD Player",CreatedOn = DateTime.Now,ModifiedOn = DateTime.Now},
        new Product{Name="GPS Unit", CategoryId =1,BuyItPrice = 120, RetailPrice = 100, UserId=1,Active=true,Description="Garmin GPS Unit",CreatedOn = DateTime.Now,ModifiedOn = DateTime.Now}
            };
            products.ForEach(s => context.Products.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();
        }

    }

}
