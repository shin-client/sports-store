using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Domain.Entities;

namespace SportsStore.Infrastructure;

public static class SeedData
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        ApplicationDbContext context =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product
                {
                    Name = "Thuyền Kayak",
                    Description = "Chiếc thuyền nhỏ cho một người chèo.",
                    Category = "Thể thao dưới nước",
                    Price = 6600000m,
                },
                new Product
                {
                    Name = "Áo phao cứu sinh",
                    Description = "Bảo hộ an toàn, kiểu dáng thời trang.",
                    Category = "Thể thao dưới nước",
                    Price = 1175000m,
                },
                new Product
                {
                    Name = "Bóng đá tiêu chuẩn",
                    Description = "Bóng đạt chuẩn kích thước và trọng lượng FIFA.",
                    Category = "Bóng đá",
                    Price = 470000m,
                },
                new Product
                {
                    Name = "Cờ góc sân",
                    Description = "Tạo vẻ chuyên nghiệp cho sân bóng của bạn.",
                    Category = "Bóng đá",
                    Price = 840000m,
                },
                new Product
                {
                    Name = "Mô hình sân vận động",
                    Description = "Sân vận động 35,000 chỗ ngồi (đóng gói phẳng).",
                    Category = "Bóng đá",
                    Price = 1908000000m,
                },
                new Product
                {
                    Name = "Mũ Tư Duy",
                    Description = "Cải thiện 75% hiệu suất hoạt động của não.",
                    Category = "Cờ Vua",
                    Price = 385000m,
                },
                new Product
                {
                    Name = "Ghế không vững",
                    Description = "Bí mật gây bất lợi cho đối thủ của bạn.",
                    Category = "Cờ Vua",
                    Price = 720000m,
                },
                new Product
                {
                    Name = "Bàn cờ người",
                    Description = "Trò chơi thú vị cho cả gia đình.",
                    Category = "Cờ Vua",
                    Price = 1800000m,
                },
                new Product
                {
                    Name = "Quân Vua Kim Cương",
                    Description = "Quân Vua mạ vàng, đính kim cương.",
                    Category = "Cờ Vua",
                    Price = 28800000m,
                },
                new Product
                {
                    Name = "Giày chạy bộ",
                    Description = "Nhẹ và thoải mái cho quãng đường dài.",
                    Category = "Chạy bộ",
                    Price = 2400000m,
                },
                new Product
                {
                    Name = "Thảm Yoga cao cấp",
                    Description = "Bề mặt chống trượt giúp giữ thăng bằng hoàn hảo.",
                    Category = "Fitness",
                    Price = 840000m,
                },
                new Product
                {
                    Name = "Bình nước giữ nhiệt",
                    Description = "Giữ lạnh đồ uống trong 24 giờ.",
                    Category = "Fitness",
                    Price = 380000m,
                }
            );

            context.SaveChanges();
        }
    }
}
