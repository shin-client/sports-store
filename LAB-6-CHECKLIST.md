# CHECKLIST THỰC HÀNH LAB 06: EF CORE & REPOSITORY PATTERN

---

## GIAI ĐOẠN 1: Cài đặt Packages & Khởi tạo DbContext (`SportsStore.Infrastructure`)

### 1. Cài đặt các gói NuGet cần thiết

- [ ] Thao tác trên project **`SportsStore.Infrastructure`**:
  - [ ] Cài đặt `Microsoft.EntityFrameworkCore.SqlServer` (Trình điều khiển EF Core cho SQL Server).
  - [ ] Cài đặt `Microsoft.EntityFrameworkCore.Tools` (Công cụ quản lý migration & CLI).

  **Lệnh CLI (Terminal):**

  ```bash
  dotnet add src/SportsStore.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer
  dotnet add src/SportsStore.Infrastructure package Microsoft.EntityFrameworkCore.Tools
  ```

---

### 2. Tạo lớp `ApplicationDbContext`

- [ ] Tạo file `ApplicationDbContext.cs` tại: `src/SportsStore.Infrastructure/ApplicationDbContext.cs`
- [ ] Kế thừa từ `DbContext` (thuộc namespace `Microsoft.EntityFrameworkCore`).
- [ ] Khai báo `using SportsStore.Domain.Entities;` để nhận diện entity `Product`.
- [ ] Khai báo Constructor nhận `DbContextOptions<ApplicationDbContext> options` và truyền `: base(options)`.
- [ ] Khai báo thuộc tính `DbSet<Product>`:

  ```csharp
  using Microsoft.EntityFrameworkCore;
  using SportsStore.Domain.Entities;

  namespace SportsStore.Infrastructure;

  public class ApplicationDbContext : DbContext
  {
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options) { }

      public DbSet<Product> Products { get; set; }
  }
  ```

---

### 3. Tạo lớp `EFProductRepository`

- [ ] Tạo file `EFProductRepository.cs` tại: `src/SportsStore.Infrastructure/EFProductRepository.cs`
- [ ] Khai báo `using SportsStore.Domain.Entities;` và `using SportsStore.Domain.Interfaces;`.
- [ ] Implement interface `IProductRepository`.
- [ ] Inject `ApplicationDbContext` qua Constructor:

  ```csharp
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
  ```

---

## GIAI ĐOẠN 2: Cấu hình WebUI & Dependency Injection (`SportsStore.WebUI`)

### 4. Thêm Connection String trong `appsettings.json`

- [ ] Mở file `src/SportsStore.WebUI/appsettings.json`.
- [ ] Thêm cấu hình `"ConnectionStrings"` (Dành cho Linux / Docker):

  ```json
  "ConnectionStrings": {
    "SportsStoreConnection": "Server=localhost,1433;Database=SportsStore;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
  ```

  _(Lưu ý: Nếu dùng Windows LocalDB, chuỗi sẽ là `"Server=(localdb)\\MSSQLLocalDB;Database=SportsStore;Trusted_Connection=True;MultipleActiveResultSets=true"`)_

---

### 5. Đăng ký Dịch vụ trong `Program.cs`

- [ ] Mở file `src/SportsStore.WebUI/Program.cs`.
- [ ] Thêm các directive `using` cần thiết:

  ```csharp
  using Microsoft.EntityFrameworkCore;
  using SportsStore.Infrastructure;
  using SportsStore.Domain.Interfaces;
  ```

- [ ] Đăng ký `ApplicationDbContext` với DI Container (sử dụng connection string):

  ```csharp
  builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("SportsStoreConnection")));
  ```

- [ ] Đổi đăng ký `IProductRepository` từ `MockProductRepository` sang `EFProductRepository`:

  ```csharp
  // Thay thế MockProductRepository bằng EFProductRepository
  builder.Services.AddScoped<IProductRepository, EFProductRepository>();
  ```

---

## GIAI ĐOẠN 3: Tạo & Seeding Dữ Liệu Ban Đầu (`SeedData.cs`)

### 6. Tạo lớp `SeedData.cs`

- [ ] Mở `src/SportsStore.Infrastructure/SportsStore.Infrastructure.csproj` và thêm `<FrameworkReference Include="Microsoft.AspNetCore.App" />` để nhận diện `IApplicationBuilder`:

  ```xml
  <ItemGroup>
    <FrameworkReference Include="Microsoft.AspNetCore.App" />
  </ItemGroup>
  ```

- [ ] Tạo file static class `SeedData.cs` tại: `src/SportsStore.Infrastructure/SeedData.cs`
- [ ] Viết hàm `public static void EnsurePopulated(IApplicationBuilder app)` để tự động nạp dữ liệu khi bảng `Products` trống:

  ```csharp
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
          ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

          if (context.Database.GetPendingMigrations().Any())
          {
              context.Database.Migrate();
          }
          else
          {
              context.Database.EnsureCreated();
          }

          if (!context.Products.Any())
          {
              context.Products.AddRange(
                  new Product { Name = "Thuyền Kayak", Description = "Chiếc thuyền nhỏ cho một người chèo.", Category = "Thể thao dưới nước", Price = 6600000m },
                  new Product { Name = "Áo phao cứu sinh", Description = "Bảo hộ an toàn, kiểu dáng thời trang.", Category = "Thể thao dưới nước", Price = 1175000m },
                  new Product { Name = "Bóng đá tiêu chuẩn", Description = "Bóng đạt chuẩn kích thước và trọng lượng FIFA.", Category = "Bóng đá", Price = 470000m },
                  new Product { Name = "Cờ góc sân", Description = "Tạo vẻ chuyên nghiệp cho sân bóng của bạn.", Category = "Bóng đá", Price = 840000m },
                  new Product { Name = "Mô hình sân vận động", Description = "Sân vận động 35,000 chỗ ngồi (đóng gói phẳng).", Category = "Bóng đá", Price = 1908000000m },
                  new Product { Name = "Mũ Tư Duy", Description = "Cải thiện 75% hiệu suất hoạt động của não.", Category = "Cờ Vua", Price = 385000m },
                  new Product { Name = "Ghế không vững", Description = "Bí mật gây bất lợi cho đối thủ của bạn.", Category = "Cờ Vua", Price = 720000m },
                  new Product { Name = "Bàn cờ người", Description = "Trò chơi thú vị cho cả gia đình.", Category = "Cờ Vua", Price = 1800000m },
                  new Product { Name = "Quân Vua Kim Cương", Description = "Quân Vua mạ vàng, đính kim cương.", Category = "Cờ Vua", Price = 28800000m },
                  new Product { Name = "Giày chạy bộ", Description = "Nhẹ và thoải mái cho quãng đường dài.", Category = "Chạy bộ", Price = 2400000m },
                  new Product { Name = "Thảm Yoga cao cấp", Description = "Bề mặt chống trượt giúp giữ thăng bằng hoàn hảo.", Category = "Fitness", Price = 840000m },
                  new Product { Name = "Bình nước giữ nhiệt", Description = "Giữ lạnh đồ uống trong 24 giờ.", Category = "Fitness", Price = 380000m }
              );

              context.SaveChanges();
          }
      }
  }
  ```

---

### 7. Kích hoạt `EnsurePopulated` trong `Program.cs`

- [ ] Trong `src/SportsStore.WebUI/Program.cs`, thêm dòng khởi tạo dữ liệu ngay trước `app.Run();`:

  ```csharp
  SeedData.EnsurePopulated(app);
  app.Run();
  ```

---

## GIAI ĐOẠN 4: Kiểm tra & Chạy ứng dụng (Verification)

### 8. Kiểm thử & Chạy ứng dụng

- [ ] Kiểm tra biên dịch dự án: `dotnet build`
- [ ] Khởi chạy Web Application: `dotnet run --project src/SportsStore.WebUI`
- [ ] Mở trình duyệt truy cập ứng dụng và kiểm tra:
  - [ ] Danh sách sản phẩm được nạp đầy đủ từ CSDL SQL Server.
  - [ ] Phân trang (Paging) và lọc danh mục (Category Filter) hoạt động chính xác qua `EFProductRepository`.
- [ ] _(Tùy chọn - Migration CLI)_:

  ```bash
  dotnet ef migrations add InitialCreate --project src/SportsStore.Infrastructure --startup-project src/SportsStore.WebUI
  dotnet ef database update --project src/SportsStore.Infrastructure --startup-project src/SportsStore.WebUI
  ```
