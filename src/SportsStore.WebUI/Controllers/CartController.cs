using Microsoft.AspNetCore.Mvc;
using SportsStore.Domain;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Interfaces;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers;

public class CartController : Controller
{
    private readonly IProductRepository _repository;
    private readonly Cart _cart;

    public CartController(IProductRepository repo, Cart cartService)
    {
        _repository = repo;
        _cart = cartService;
    }

    public ViewResult Index(string? returnUrl)
    {
        return View(new CartIndexViewModel
        {
            Cart = _cart,
            ReturnUrl = returnUrl ?? "/"
        });
    }

    public RedirectToActionResult AddToCart(int productId, string? returnUrl)
    {
        Product? product = _repository.Products
            .FirstOrDefault(p => p.ProductID == productId);

        if (product != null)
        {
            _cart.AddItem(product, 1);
        }

        return RedirectToAction("Index", new { returnUrl });
    }

    public RedirectToActionResult RemoveFromCart(int productId, string? returnUrl)
    {
        Product? product = _repository.Products
            .FirstOrDefault(p => p.ProductID == productId);

        if (product != null)
        {
            _cart.RemoveLine(product);
        }

        return RedirectToAction("Index", new { returnUrl });
    }
}
