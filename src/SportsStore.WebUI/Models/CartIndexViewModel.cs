using SportsStore.Domain;

namespace SportsStore.WebUI.Models;

public class CartIndexViewModel
{
    public required Cart Cart { get; set; }
    public string ReturnUrl { get; set; } = "/";
}
