using MengGrocery.DAL;
using MengGrocery.Models;
using Microsoft.AspNetCore.Mvc;
 
[Area("Product")]
[Route("Product")]
public class ProductController : Controller
{
    private readonly IProductQuery _productQuery;

    public ProductController(IProductQuery productQuery)
    {
        _productQuery = productQuery;
    }



    public IActionResult Index()
    {
        var products = _productQuery.GetProduct();
        var model = new ProductViewModel
        {
            Products = products.ToList()
        };
        return View(model);
    }

    
}   
