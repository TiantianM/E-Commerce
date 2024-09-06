
using System.ComponentModel.DataAnnotations;
using FoolProof.Core;

public class CheckoutViewModel
{
        [Required]
        [Display(Name = "Shipping First Name")]
        public string ShippingFirstName { get; set; }
        [Required]
        [Display(Name = "Shipping Last Name")]
        public string ShippingLastName { get; set; }
        [Required]
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        [Required]
        [Display(Name = "Shipping City")]
        public string ShippingCity { get; set; }
        [Required]
        [Display(Name = "Shipping State")]
        public string ShippingState { get; set; }
        [Required]
        [Display(Name = "Shipping Zip Code")]
        public string ShippingZipCode { get; set; }
        [Required]
        [Display(Name = "Shipping Country")]
        public string ShippingCountry { get; set; }
        [Required]
        [Display(Name = "Shipping Phone Number")]
        public string ShippingPhoneNumber { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Billing Same as Shipping")]
        public bool BillingSameAsShipping { get; set; }


        [Required]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Required]
        [Display(Name = "Expiration Date")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$", ErrorMessage = "Invalid expiration date")]
        public string ExpirationDate { get; set; }




        //billing
        [RequiredIf("BillingSameAsShipping", false)]
        [Display(Name = "Billing First Name")]
        public string? BillingFirstName { get; set; }
        [RequiredIf("BillingSameAsShipping", false)]
        [Display(Name = "Billing Last Name")]
        public string? BillingLastName { get; set; }
        [RequiredIf("BillingSameAsShipping", false)]
        [Display(Name = "Billing Address")]
        public string? BillingAddress { get; set; }
        [RequiredIf("BillingSameAsShipping", false)]
        [Display(Name = "Billing City")]
        public string? BillingCity { get; set; }
        [RequiredIf("BillingSameAsShipping", false)]
        [Display(Name = "Billing State")]
        public string? BillingState { get; set; }
        [RequiredIf("BillingSameAsShipping", false)]
        [Display(Name = "Billing Zip Code")]
        public string? BillingZipCode { get; set; }
        //required if BillingSameAsShipping is false
        [RequiredIf("BillingSameAsShipping", false)]
        [Display(Name = "Billing Country")]
        public string? BillingCountry { get; set; }
        [RequiredIf("BillingSameAsShipping", false)]
        [Display(Name = "Billing Phone Number")]
        public string? BillingPhoneNumber { get; set; }
        

        public List<string>? Countries { get; set; }
}