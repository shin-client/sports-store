using SportsStore.Domain.Entities;
using SportsStore.Domain.Interfaces;

namespace SportsStore.Application.Concrete;

public class MockProductRepository : IProductRepository
{
    public IQueryable<Product> Products => new List<Product>
    {
        new() { ProductID = 1, Name = "Quả bóng đá", Description = "Bóng chuẩn thi đấu World Cup", Price = 25.00m, Category = "Bóng đá" },
        new() { ProductID = 2, Name = "Áo đấu tuyển Việt Nam", Description = "Vải thoáng khí, co giãn tốt", Price = 15.00m, Category = "Quần áo" },
        new() { ProductID = 3, Name = "Giày chạy bộ", Description = "Đế êm, hỗ trợ phản lực", Price = 49.99m, Category = "Chạy bộ" },
        new() { ProductID = 4, Name = "Kính bơi chống sương mù", Description = "Nhìn rõ dưới nước", Price = 12.50m, Category = "Dưới nước" }
    }.AsQueryable();
}