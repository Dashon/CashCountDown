// JavaScript source code

$(document).ready(function () {


    // Variable to hold auction data
    var auctions = '';
    var auctionObjects = new Array();
    isFuture = false; isEnded = false;

    // Collecting auction data, the layer id and auction id
    $('.auction-Product').each(function () {
        var auctionId = $(this).attr('id');
        var auctionTitle = $(this).attr('title');

        if ($('#' + auctionId + ' .countdown').length) {

            // collect the id for post data
            // auctions = auctions + auctionId + '=' + auctionTitle + '&';
            auctions = auctions + auctionTitle + '&';
            // collect the object
            auctionObjects[auctionId] = $('#' + auctionId);
            auctionObjects[auctionId]['flash-elements'] = $('#' + auctionId + ' .countdown, #' + auctionId + ' .bid-price, #' + auctionId + ' .bid-bidder, #' + auctionId + ' .bid-savings-price, #' + auctionId + ' .bid-savings-percentage, #' + auctionId + ' .closes-on');
            auctionObjects[auctionId]['countdown'] = $('#' + auctionId + ' .countdown');
            auctionObjects[auctionId]['closes-on'] = $('#' + auctionId + ' .closes-on');
            auctionObjects[auctionId]['bid-bidder'] = $('#' + auctionId + ' .bid-bidder');
            auctionObjects[auctionId]['bid-button'] = $('#' + auctionId + ' .bid-button');
            auctionObjects[auctionId]['bid-button-a'] = $('#' + auctionId + ' .bid-button a');
            auctionObjects[auctionId]['bid-button-p'] = $('#' + auctionId + ' .bid-button p');
            auctionObjects[auctionId]['bid-price'] = $('#' + auctionId + ' .bid-price');
            auctionObjects[auctionId]['flashers'] = $('#' + auctionId + ' .flashers');
            auctionObjects[auctionId]['bid-price2'] = $('#' + auctionId + ' .bid-price2');
            auctionObjects[auctionId]['buy-it-now'] = $('#' + auctionId + ' .price_bin');
            auctionObjects[auctionId]['bid-price-fixed'] = $('#' + auctionId + ' .bid-price-fixed');
            auctionObjects[auctionId]['bid-loading'] = $('#' + auctionId + ' .bid-loading');
            auctionObjects[auctionId]['bid-message'] = $('#' + auctionId + ' .bid-message');
            auctionObjects[auctionId]['bid-flash'] = $('#' + auctionId + ' .bid-flash');
            auctionObjects[auctionId]['bid-savings-price'] = $('#' + auctionId + ' .bid-savings-price');
            auctionObjects[auctionId]['bid-savings-percentage'] = $('#' + auctionId + ' .bid-savings-percentage');
            auctionObjects[auctionId]['bid-bookbidbutler'] = $('#' + auctionId + ' .bid-bookbidbutler');
            auctionObjects[auctionId]['bid-increment'] = $('#' + auctionId + ' .bid-increment');
            auctionObjects[auctionId]['price-increment'] = $('#' + auctionId + ' .price-increment');

            auctionObjects[auctionId]['bid-histories'] = $('#bidHistoryTable' + auctionTitle);
            auctionObjects[auctionId]['bid-histories-p'] = $('#bidHistoryTable' + auctionTitle + ' p');
            auctionObjects[auctionId]['bid-histories-tbody'] = $('#bidHistoryTable' + auctionTitle + ' tbody');

            auctionObjects[auctionId]['bid-quota-value'] = $('#' + auctionId + ' .bid_quota_value');
        }
    });

    // additional object
    var bidOfficialTime = $('.bid-official-time');
    var bidBalance = $('.bid-balance');
    var price = '';
    var priceFixed = '';
    var getstatus_url_time;
    var getstatus_url;

    if ($('.bid-histories').length) {
        //TODO:Create and update statusupdate function
        getstatus_url = '/auctions/statusupdate?h=yes&tt=';
    } else {
        getstatus_url = '/auctions/statusupdate?tt=';
    }

    function convertToNumber(sourceString) {
        if (sourceString == null) return '0.00';
        // var num = sourceString.replace(/&#[0-9]{1,};/gi, "").replace(/&[a-z]{1,};/gi, "").replace(/[a-zA-Z]+/gi, "").replace(/[^0-9\,\.]/gi, "");
        return sourceString;
    }

    // Do the loop when auction available only
    if (auctions) {
        setInterval(function () {
            getstatus_url_time = getstatus_url + new Date().toUTCString();
            //todo insert linq.js or similar function  to replace ajax


            $.ajax({
                url: getstatus_url_time,
                dataType: 'json',
                type: 'POST',
                data: auctions,
                success: function (data) {
                    if (data[0]) {
                        if (data[0].serverTimeString) {
                            if (bidOfficialTime.html()) {
                                bidOfficialTime.html(data[0].serverTimeString);
                            }
                        }

                        if (data[0].Balance) {
                            bidBalance.html("Bids Remaining: " + data[0].Balance);

                        }

                    }

                    $.each(data, function (i, Product) {
                        if (typeof (auctionObjects[Product.Auction]) == 'undefined') return true; //continue


                        if (auctionObjects[Product.Auction]['bid-price'].length > 1) {
                            auctionObjects[Product.Auction]['bid-price'].each(function () {
                                price = $(this).html();
                            });
                        } else {
                            price = auctionObjects[Product.Auction]['bid-price'].html();
                        }

                        if (Product.price_increment) {
                            auctionObjects[Product.Auction]['price-increment'].html("$"+Product.price_increment);
                        }

                        if (Product != '') {
                            if (auctionObjects[Product.Auction]['bid-bidder'].html() != Product.WinningBidder) {
                                auctionObjects[Product.Auction]['bid-bidder'].html(Product.WinningBidder);
                            }
                        }
                        //  auctionObjects[Product.Auction]['buy-it-now'].html(item.Auction.buy_it_now);

                        if (price != "$" + Product.Price && price != '0.00') {
                            auctionObjects[Product.Auction]['bid-price'].html("$" + Product.Price);
                            auctionObjects[Product.Auction]['bid-price2'].html("$" + Product.Price);
                            auctionObjects[Product.Auction]['bid-price-fixed'].html("$" + Product.Price);
                            auctionObjects[Product.Auction]['bid-savings-percentage'].html(Product.savingsPercentage + "%");
                            auctionObjects[Product.Auction]['bid-savings-price'].html("$" + Product.savingsPrice);

                            if (auctionObjects[Product.Auction]['bid-flash']) {
                                auctionObjects[Product.Auction]['bid-flash'].html("").show(1).animate({
                                    opacity: 1.0
                                }, 2000).hide(1);
                            }
                            if (auctionObjects[Product.Auction]['flashers']) {
                                auctionObjects[Product.Auction]['flashers'].effect("highlight", {}, 1500);
                            }
                        }

                        if (auctionObjects[Product.Auction]['bid-histories'].length) {

                            if (auctionObjects[Product.Auction]['bid-histories-p'].html()) {
                                auctionObjects[Product.Auction]['bid-histories-p'].remove();
                            }

                            auctionObjects[Product.Auction]['bid-histories-tbody'].empty();

                            if (Product.BidHistory) {
                                $.each(Product.BidHistory, function (n, tRow) {

                                    var row = '<tr><td>' + tRow.CreatedOn + '</td><td>' + tRow.UserId + '</td><td>' + tRow.IsAutoBid + '</td></tr>';
                                    auctionObjects[Product.Auction]['bid-histories-tbody'].append(row);
                                });
                            }

                            //auctionObjects[Product.Auction]['closes-on'].html(item.Auction.closes_on);
                       
                        }


                        var skip_timer = false;

                        if (!skip_timer) {

                            if (Product.TimeLeft.TotalSeconds > 0) {
                                auctionObjects[Product.Auction]['countdown'].html(Product.TimeLeft.Hours + ":" + Product.TimeLeft.Minutes + ":" + Product.TimeLeft.Seconds);

                                if (Product.TimeLeft.TotalSeconds < 10) {
                                    auctionObjects[Product.Auction]['countdown'].css('color', '#ff0000');
                                } else {
                                    auctionObjects[Product.Auction]['countdown'].removeAttr('style');
                                }
                            }
                        }

                        if (auctionObjects[Product.Auction]['bid-button-p'].html()) {
                            auctionObjects[Product.Auction]['bid-button-a'].show();
                            auctionObjects[Product.Auction]['bid-button-p'].remove();
                        }


                        if (Product.isFuture == 0) {
                            auctionObjects[Product.Auction]['countdown'].html('Ended');
                            auctionObjects[Product.Auction]['bid-button'].hide();
                            auctionObjects[Product.Auction]['bid-bookbidbutler'].hide();

                            if (isEnded == false && auctionObjects.length == 1) {
                                isEnded = true;
                                window.location.reload();
                                return;
                            }
                        }


                    });
                },

            });
        }, 1000);
    } else {
        if (bidOfficialTime.length) {
            setInterval(function () {
                //todo:Write ourtime function to recieve server time
                var gettime = 'auctions/ourtime?' + new Date().getTime();
                $.ajax({
                    url: gettime,
                    success: function (data) {
                        bidOfficialTime.html(data);
                    }
                });
            }, 1000);
        }
    }
    function AddAntiForgeryToken(data) {
        data.__RequestVerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();
        return data;
    };

    // Function for bidding
    $('.bid-button-link').click(function () {

        var auctionElement = 'auction_' + $(this).attr('title');

        auctionObjects[auctionElement]['bid-button'].hide(1);
        auctionObjects[auctionElement]['bid-loading'].show(1);


        var params = $(this).attr('href') + '&tt=' + new Date().getTime();

        var unique_price = $('#unique_price_' + $(this).attr('title'));

        if (unique_price.length > 0) {
        } $.ajax({
            url: "/auctions/PlaceBidAjax",
            dataType: 'json',
            type: "POST",
            data: AddAntiForgeryToken({ id: parseInt($(this).attr("title")), tt: new Date().getTime() }),
            beforeSend: function () { },
            complete: function (data) { },
            success: function (data) {
                if (unique_price.length > 0) {
                    unique_price.val('');
                }
                auctionObjects[auctionElement]['bid-message'].html(data)
                                .show(1)
                                .animate({
                                    opacity: 1.0
                                }, 2000)
                                .fadeOut("slow");
                auctionObjects[auctionElement]['bid-button'].show(1);
                auctionObjects[auctionElement]['bid-loading'].hide(1);
            }
        });

        return false;
    });

    // Function to check limit and change the icon whenever it's changed
    // Run only when bid icon available
    //todo:research this
    if ($('.Xbid-limit-icon').length) {
        setInterval(function () {
            var count = $('.bid-limit-icon').length
            if (count > 0) {

                $.ajax({
                    url: '/limits/getlimitsstatus/?ms=' + new Date().getTime(),
                    dataType: 'json',
                    success: function (data) {
                        if (data) {
                            $('.bid-limit-icon').each(function (i) {
                                if (data[i].image) {
                                    $(this).attr('src', '/img/' + data[i].image);
                                }
                            });
                        }
                    }
                });
            }
        }, 30000);
    }

    if ($('.productImageThumb').length) {
        $('.productImageThumb').click(function () {
            $('.productImageMax').fadeOut('fast').attr('src', $(this).attr('href')).fadeIn('fast');
            return false;
        });
    }

    if ($('#CategoryId').length) {
        $('#CategoryId').change(function () {
            document.location = '/categories/view/' + $('#CategoryId option:selected').attr('value');
        });
    }

    if ($('#myselectbox').length) {
        $('#myselectbox').change(function () {
            document.location = '/categories/view/' + $('#myselectbox option:selected').attr('value');
        });
    }
});

