using CashCountDown.Models;
using CashCountDown.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CashCountDown.Helpers
{
    public class AuctionHelpers
    {
        private AuctionRepository AuctionRepo = new AuctionRepository();

        
        public Boolean Authorize(Authorizer Auth)
        {
            return true;
        }

        public UserProfile WinningBidder(int AuctionId)
        {
            return AuctionRepo.winningBidder(AuctionId);
        }


        public IEnumerable<BidHistory> BidHistory(int ProductId)
        {
            IEnumerable<BidHistory> bids = AuctionRepo.BidHistory(ProductId);
            return bids;
        }

        //i dont think you can do this without MARS..be sure to include all the data in the incoming shipment
        public Boolean MarkShipped(Shipment Shipment)
        {
            return AuctionRepo.markShipped(Shipment);
        }


        //i dont think you can do this without MARS..be sure to include all the data in the incoming shipment
        public Boolean UpdateShipment(Shipment shipment)
        {
            return AuctionRepo.updateShipment(shipment);

        }
        //i dont think you can do this without MARS..be sure to include all the data in the incoming media
        public Boolean AddMedia(Media newMedia)
        {
            return AuctionRepo.addMedia(newMedia);

        }
        //i dont think you can do this without MARS..
        public Boolean RemoveMedia(int MediaId)
        {
            return AuctionRepo.removeMedia(MediaId);
        }


        public int GetBidBalance(int Userid)
        {
            return AuctionRepo.getBidBalance(Userid);
        }


        //i dont think you can do this without MARS..be sure to include all the data in the incoming bidBackageContents
        public Boolean LoadBidPackage(LoadBidPackage bidpackageContents)
        {
            return AuctionRepo.loadBidPackage(bidpackageContents);

        }

        public Boolean SubmitPayPal(PayPalSubmit PayPalSubmitContents)
        {   //Enter PayPal Command
            return AuctionRepo.submitPayPal(PayPalSubmitContents);
        }

        public int GetItemCost(int AuctionId)
        {
            return AuctionRepo.gGetItemCost(AuctionId);
        }

    }
}