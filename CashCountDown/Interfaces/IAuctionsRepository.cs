﻿using System;
using CashCountDown.Models;
using System.Web.Mvc;
using System.Linq;

namespace CashCountDown.Interfaces
{

    interface IAuctionRepository
    {
        //Auction FindBy(int fooId);
        Auction_Details get(int id);
        Auction_Edit getEdit(int id);
        Boolean edit(Auction_Edit edit);
        ToDeactivate getDeactivate(int id);
        Auction_Create getCreate();
        Boolean create(Auction_Create create);
        IQueryable<BidHistory> BidHistory(int AuctionId);

        IQueryable<UpdateResult> statusupdate(DateTime tt, Array ids);

        string placeBid(int id);
        bool submitOrder(SubmitOrder OrderContents);

        string addtoList(WishList_Auction wish);


        Boolean markShipped(Shipment Shipment);
        Boolean updateShipment(Shipment Shipment);

        Boolean addMedia(Media newMedia);
        Boolean removeMedia(int MediaId);

        int gGetItemCost(int AuctionId);
        //create viewModel
        UserProfile winningBidder(int AuctionId);


        //users
        int getBidBalance(int Userid);
        Boolean loadBidPackage(LoadBidPackage bidpackageContents);
        Boolean submitPayPal(PayPalSubmit PayPalSubmitContents);

    }

}
