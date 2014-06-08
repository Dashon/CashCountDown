using CashCountDown.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Diagnostics;

namespace CashCountDown
{
    public class AppService
    {
        public static void Main()
        {
            AutoResetEvent reset = new AutoResetEvent(false);
            OverLord overLord = new OverLord(5);

            // Invoke methods for the timer via a Delegate
            TimerCallback timerDelegate = new TimerCallback(overLord.Run);

            // Create a timer that signals the delegate to invoke 
            // Check status after one second, and then every 1/4 second
            Debug.WriteLine("Creating the timer.\n", DateTime.Now.ToString("h:mm:ss.fff"));
            //Timer stateTimer = new Timer(timerDelegate, reset, 0,1000);

        }
    }

    class OverLord
    {
        private readonly CashCountDownContext db = new CashCountDownContext();
        int invokeCount, maxCount;

        public OverLord(int count)
        {
            invokeCount = 90;
            maxCount = count;
        }

        // This method is called by the timer delegate.
        public void Run(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            Bid newbid = new Bid
            {
                Amount = invokeCount,
                AuctionId = 1,
                CreatedOn = DateTime.Now,
                UserId = 1,
                IsAutoBid = true
            };
          

            if (invokeCount == maxCount)
            { 
                db.Bids.Add(newbid);
                db.SaveChanges();
                // Reset the counter and signal Main.
                //  invokeCount = 0;
                //  autoEvent.Set();
            }
        }
    }
}