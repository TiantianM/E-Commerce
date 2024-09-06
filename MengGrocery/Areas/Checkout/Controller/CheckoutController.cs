using MengGrocery.DAL;
using MengGrocery.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using MengGrocery.Helpers;



[Area("Checkout")]
[Route("Checkout")]
public class CheckoutController : Controller
{
    private readonly CustomUserManager _userManager;
    private readonly IOrderQuery _orderQuery;
    private readonly CustomSignInManager _signInManager;
    private readonly ICookieHelper _cookieHelper;
    private readonly ICartQuery _cartQuery;


    public CheckoutController(IOrderQuery orderQuery, CustomUserManager userManager, CustomSignInManager signInManager, ICookieHelper cookieHelper, ICartQuery cartQuery)
        {
            _orderQuery = orderQuery;
            _userManager = userManager;
            _signInManager = signInManager;
            _cookieHelper = cookieHelper;
            _cartQuery = cartQuery;
        }

    
    public IActionResult Index()
        {
            var model = new CheckoutViewModel();
            if (_signInManager.IsSignedIn(User))
            {
                model.Email = User.Identity?.Name;
            }

            
            model.Countries = new List<string>
            {
               "United States",
               "Canada",
               "Mexico",
               "China",
               "Japan",
               "Korea",
               "Germany",
               "France"
            };
            return View(model);
        }


    [HttpPost]
    public IActionResult Submit(CheckoutViewModel model)
    {
        if(ModelState.IsValid)
        {
            var cartId = _cookieHelper.GetCookie("CartId");
            if (string.IsNullOrEmpty(cartId))
            {
                ModelState.AddModelError("", "Cart is Empty");
                return View("Index", model);
            }

            var cart = _cartQuery.GetCart(cartId);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                ModelState.AddModelError("", "Cart is Empty");
                return View("Index", model);
            }

          

            var order = new Order()
            {
                CartID = cart.CartId,
                CreatedAt = DateTime.Now,
                OrderStatus = "Pending",
                TotalAmount = cart.CartItems.Sum(x => x.Quantity * x.PriceAtAdd),
                Tax = 0,
                ShippingPrice = 0,
                Coupon = 0,
                    
            };

            if (model.BillingSameAsShipping)
                {
                    model.BillingFirstName = model.ShippingFirstName;
                    model.BillingLastName = model.ShippingLastName;
                    model.BillingAddress = model.ShippingAddress;
                    model.BillingCity = model.ShippingCity;
                    model.BillingState = model.ShippingState;
                    model.BillingZipCode = model.ShippingZipCode;
                    model.BillingPhoneNumber = model.ShippingPhoneNumber;
                    model.BillingCountry = model.ShippingCountry;

                }

                var OrderCustomer = new OrderCustomer()
                {
                    ShippingAddress = model.ShippingAddress,
                    ShippingCity = model.ShippingCity,
                    ShippingFirstName = model.ShippingFirstName,
                    ShippingLastName = model.ShippingLastName,
                    ShippingZipCode = model.ShippingZipCode,
                    ShippingState = model.ShippingState,
                    ShippingCountry = model.ShippingCountry,
                    ShippingPhoneNumber = model.ShippingPhoneNumber,
                    BillingAddress = model.BillingAddress,
                    BillingCity = model.BillingCity,
                    BillingFirstName = model.BillingFirstName,
                    BillingLastName = model.BillingLastName,
                    BillingZipCode = model.BillingZipCode,
                    BillingPhoneNumber = model.BillingPhoneNumber,
                    BillingState = model.BillingState,
                    BillingCountry = model.BillingCountry,
                    Email = model.Email,
                };


            var items = cart.CartItems.Select(x => new OrderItem()
                {
                    ProductID = x.ProductID,
                    Quantity = x.Quantity,
                    UnitPrice = (decimal) x.PriceAtAdd,
                }).ToList();

                var result = _orderQuery.PlaceOrder(order, items, OrderCustomer);

                if (result.Success)
                {
                    return RedirectToAction("Index", "Confirmation", new { area = "Confirmation" });
                }
                else
                {
                    ModelState.AddModelError("", "Something wrong with the order, please try again");
                    return View("Index", model);
                }


            
        }

        ModelState.AddModelError("", "Please fill all required fields");

        return View("Index", model);
    }
}  




