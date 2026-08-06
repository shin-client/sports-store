using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Models;

public class ProductsListViewModel
{
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    public PagingInfo PagingInfo { get; set; } = new();
    public string? CurrentCategory { get; set; }
}
