using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Domain.Interfaces;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly IProductRepository _repository;

    public HomeController(IProductRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var products = _repository.Products.ToList();
        return View(products);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
