using System;
using CashCountDown.Models;
using System.Web.Mvc;
using System.Linq;
using System.Data.Entity;


namespace CashCountDown.Interfaces
{
    interface IAutoBidRepository
    {
        bool EditAutoBid(AutoBid autobid);
        bool DeactivateAutoBid(int id);
        bool NewAutoBid(AutoBid autobid);
        bool DoAutobids(int id);
    }
}