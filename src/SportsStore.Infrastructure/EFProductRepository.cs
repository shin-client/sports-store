using SportsStore.Domain.Entities;
using SportsStore.Domain.Interfaces;

namespace SportsStore.Infrastructure;

public class EFProductRepository : IProductRepository
{
    private ApplicationDbContext _context;

    public EFProductRepository(ApplicationDbContext ctx)
    {
        _context = ctx;
    }

    public IQueryable<Product> Products => _context.Products;
}
