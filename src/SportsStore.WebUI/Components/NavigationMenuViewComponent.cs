using Microsoft.AspNetCore.Mvc;
using SportsStore.Domain.Interfaces;

namespace SportsStore.WebUI.Components;

public class NavigationMenuViewComponent : ViewComponent
{
    private readonly IProductRepository _repository;

    public NavigationMenuViewComponent(IProductRepository repository)
    {
        _repository = repository;
    }

    public IViewComponentResult Invoke()
    {
        ViewBag.SelectedCategory = RouteData?.Values["category"];

        var categories = _repository.Products.Select(p => p.Category).Distinct().OrderBy(c => c);

        return View(categories);
    }
}
