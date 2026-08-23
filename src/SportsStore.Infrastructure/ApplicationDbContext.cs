using Microsoft.EntityFrameworkCore;
using SportsStore.Domain.Entities;

namespace SportsStore.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Product> Products => Set<Product>();
}
