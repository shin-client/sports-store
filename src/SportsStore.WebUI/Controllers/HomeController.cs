using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Domain.Interfaces;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly IProductRepository _repository;
    public int PageSize = 6;

    public HomeController(IProductRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index(string? category, int productPage = 1)
    {
        return View(new ProductsListViewModel
        {
            Products = _repository.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductID)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize),
            PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = category == null
                    ? _repository.Products.Count()
                    : _repository.Products.Where(e => e.Category == category).Count()
            },
            CurrentCategory = category
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
