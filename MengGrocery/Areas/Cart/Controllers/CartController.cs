using MengGrocery.DAL;
using MengGrocery.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using MengGrocery.Helpers;

[Area("Cart")]
[Route("Cart")]

public class CartController : Controller
{
    private readonly ICartQuery _cartQuery;

    public CartController(ICartQuery cartQuery)
    {
        _cartQuery = cartQuery;

    }

    

    public IActionResult Index()
    {
        //get carguid from cookie
        var cartGuid = Request.Cookies["CartGuid"];
        if (string.IsNullOrEmpty(cartGuid))
        {
            cartGuid = Guid.NewGuid().ToString();
            Response.Cookies.Append("CartGuid", cartGuid);
        }


        var cart = _cartQuery.GetCartByGuid(cartGuid);
        
        var model = new CartViewModel
        {
           
            CartItems = cart?.CartItems,

        };
        return View(model);
    }

    [Route("AddToCart/{id?}")]
    [HttpGet]
    public IActionResult AddToCart(int productId, int quantity)
    {

        var cartGuidId = Request.Cookies["CartGuid"];
        if(string.IsNullOrEmpty(cartGuidId))
        {
            return View("Index");
        }
        _cartQuery.AddCartItem(cartGuidId, productId, quantity);
        var model = GetCartViewModel(cartGuidId);
        return View("Index", model);
    }

    [Route("RemoveItem/{id?}")]
    public IActionResult RemoveItem(int productId)
    {
        var cartGuidId = Request.Cookies["CartGuid"];
        if (string.IsNullOrEmpty(cartGuidId))
        {
            return View("Index");
        }

        _cartQuery.DeleteCartItem(cartGuidId, productId);
        var model = GetCartViewModel(cartGuidId);
        return View("Index", model);
    }

    // [Route("AjaxRemove/{id?}")]
    // public IActionResult AjaxRemove(int productId)
    // {
    //     var cartGuidId = Request.Cookies["CartGuid"];
    //     if (string.IsNullOrEmpty(cartGuidId))
    //     {
    //         return View("Index");
    //         //return Json(View("Index"));
    //     }

    //     _cartQuery.DeleteCartItem(cartGuidId, productId);
    //     var model = GetCartViewModel(cartGuidId);
    //     return PartialView("_CartList", model);
    //     //return Json(PartialView("_CartList", model));
    // }

    [Route("UpdateItem")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateItem(int productId, int quantity)
    {
        var cartGuidId = Request.Cookies["CartGuid"];
        if (string.IsNullOrEmpty(cartGuidId))
        {
            return View("Index");
        }
        _cartQuery.UpdateCartItem(cartGuidId, productId, quantity);
        var model = GetCartViewModel(cartGuidId);
        return View("Index", model);
    }

    // [Route("AjaxUpdateQuantity")]
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public IActionResult AjaxUpdateQuantity(int productId, int quantity)
    // {
    //     var cartGuidId = Request.Cookies["CartGuid"];
    //     if (string.IsNullOrEmpty(cartGuidId))
    //     {
    //         return View("Index");
    //     }
    //     _cartQuery.UpdateCartItem(cartGuidId, productId, quantity);
    //     var model = GetCartViewModel(cartGuidId);
    //     return PartialView("_CartList", model);
    // }

    // [Route("AjaxUpdateQuantityJson")]
    //     [HttpPost]
    //     public JsonResult AjaxUpdateQuantityJson([FromBody] CartUpdateJson cartUpdateJson)
    //     {
    //         if(cartUpdateJson == null || cartUpdateJson.ProductId == 0 || cartUpdateJson.Quantity == 0)
    //         {
    //             return new JsonResult(new { success = false}); 
    //         }
    //         var cartGuidId = Request.Cookies["CartGuid"];

    //         _cartQuery.UpdateCartItem(cartGuidId, cartUpdateJson.ProductId, cartUpdateJson.Quantity);
    //         // _cartQuery.UpdateCart(GetCartId(), cartUpdateJson.ProductId, cartUpdateJson.Quantity);

    //         var model = GetCartViewModel(cartGuidId);

    //         return new JsonResult(new { success = true });
    //     }


    private CartViewModel GetCartViewModel(string cartGuid)
    {
        var cart = _cartQuery.GetCartByGuid(cartGuid);
        return new CartViewModel
        {
            
            CartItems = cart?.CartItems
        };
    }


    [Route("GetCartItemCount")]
    [HttpGet]
    public JsonResult GetCartItemCount()
    {
        var cartGuid = Request.Cookies["CartGuid"];
        if (string.IsNullOrEmpty(cartGuid))
        {
            return Json(0);
        }
        var count = _cartQuery.GetCartItemCount(cartGuid);
        return Json(count);
    }  

    

    


}