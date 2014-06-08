using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace CashCountDown.Models
{

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalProfileModel
    {

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle")]
        public string MiddleName { get; set; }


        [Display(Name = "Last name")]
        public string LastName { get; set; }


        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle")]
        public string MiddleName { get; set; }


        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
    public class SubmitOrder
    {
        [Required]
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public int BidPackageId { get; set; }
    }


    public class LoadBidPackage
    {
        [Required]
        public int PackageId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
    public class PlaceBid
    {
        [Required]
        public int AuctionId { get; set; }
        [ForeignKey("AuctionId")]
        public virtual Auction Auction { get; set; }
        [Required]
        public int UserId { get; set; }
        public int Amount { get; set; }

    }

   [Serializable]
    public class UpdateResult
    {
        public string Auction { get; set; }
        public int Balance { get; set; }
        public DateTime serverTimeString { get; set; }
        public bool future { get; set; }
        public int Price { get; set; }
        public bool CanBid { get; set; }
        public TimeSpan TimeLeft { get; set; }
        public double savingsPercentage { get; set; }
        public double savingsPrice { get; set; }
        public int price_increment { get; set; }
        public String WinningBidder { get; set; }
        public List<Bid> BidHistory { get; set; }

    }
   


}
