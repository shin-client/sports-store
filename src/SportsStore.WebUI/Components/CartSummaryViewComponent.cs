using Microsoft.AspNetCore.Mvc;
using SportsStore.Domain;

namespace SportsStore.WebUI.Components;

public class CartSummaryViewComponent : ViewComponent
{
    private readonly Cart _cart;

    public CartSummaryViewComponent(Cart cartService)
    {
        _cart = cartService;
    }

    public IViewComponentResult Invoke()
    {
        return View(_cart);
    }
}
