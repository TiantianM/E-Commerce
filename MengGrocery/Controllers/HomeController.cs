using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MengGrocery.Models;
using MengGrocery.DAL;

namespace MengGrocery.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // var query = new ProductQuery();
        // var product = query.GetProduct();
        // ViewData["Product"] = product.ProductName;
        return View();
        
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

