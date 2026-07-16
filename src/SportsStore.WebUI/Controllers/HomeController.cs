using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SportsStore.WebUI.Models;
using SportsStore.Domain.Interfaces;

namespace SportsStore.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _productRepo;

    public HomeController(ILogger<HomeController> logger, IProductRepository productRepo)
    {
        _logger = logger;
        _productRepo = productRepo;
    }

    public IActionResult Index()
    {
        var products = _productRepo.Products.ToList();
        return View(products);
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
