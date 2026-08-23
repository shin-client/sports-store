# CHECKLIST THỰC HÀNH LAB 07: QUẢN LÝ SCHEMA VỚI MIGRATIONS & SEED DATA

---

## GIAI ĐOẠN 1: Cấu hình Local Tool & Packages (`dotnet-ef`)

### 1. Cài đặt công cụ CLI `dotnet-ef` ở cấp độ Local (Tránh rác máy)

- [ ] Tạo file tool-manifest local tại thư mục gốc dự án:

  ```bash
  dotnet new tool-manifest
  ```

- [ ] Cài đặt package `dotnet-ef` local:

  ```bash
  dotnet tool install dotnet-ef
  ```

- [ ] Kiểm tra công cụ đã sẵn sàng:

  ```bash
  dotnet ef --version
  ```

---

### 2. Cài đặt Package `Microsoft.EntityFrameworkCore.Design` vào WebUI

- [ ] Thêm package `Microsoft.EntityFrameworkCore.Design` vào dự án Startup (`SportsStore.WebUI`):

  ```bash
  dotnet add src/SportsStore.WebUI/SportsStore.WebUI.csproj package Microsoft.EntityFrameworkCore.Design
  ```

---

## GIAI ĐOẠN 2: Khởi tạo & Áp dụng Migration đầu tiên (`InitialCreate`)

### 3. Tạo Migration ban đầu

- [ ] Chạy lệnh tạo migration `InitialCreate`:

  ```bash
  dotnet ef migrations add InitialCreate --project src/SportsStore.Infrastructure --startup-project src/SportsStore.WebUI
  ```

- [ ] Kiểm tra thư mục `src/SportsStore.Infrastructure/Migrations/` được sinh ra tự động chứa:
  - [ ] `..._InitialCreate.cs` (Chứa phương thức `Up()` và `Down()`).
  - [ ] `ApplicationDbContextModelSnapshot.cs` (Ảnh chụp cấu trúc CSDL hiện tại).

---

### 4. Cập nhật CSDL (`Update-Database`)

- [ ] Áp dụng migration vừa tạo lên CSDL SQL Server:

  ```bash
  dotnet ef database update --project src/SportsStore.Infrastructure --startup-project src/SportsStore.WebUI
  ```

- [ ] Xoá CSDL cũ nếu lỗi lặp bảng:

  ```bash
  dotnet ef database drop --project src/SportsStore.Infrastructure --startup-project src/SportsStore.WebUI --force
  ```

- [ ] Kiểm tra CSDL:
  - [ ] Bảng `__EFMigrationsHistory` được tạo để theo dõi các bản migration.
  - [ ] Bảng `Products` được tạo đúng cấu trúc schema từ `ApplicationDbContext`.

---

## GIAI ĐOẠN 3: Chuẩn hóa Seeding Dữ liệu (`SeedData.cs`)

### 5. Cập nhật logic trong `SeedData.cs`

- [ ] Mở file `src/SportsStore.Infrastructure/SeedData.cs`.
- [ ] Loại bỏ nhánh `else { context.Database.EnsureCreated(); }` để hoàn toàn phụ thuộc vào Migrations:

  ```csharp
  if (context.Database.GetPendingMigrations().Any())
  {
      context.Database.Migrate();
  }

  if (!context.Products.Any())
  {
      // ... AddRange dữ liệu mẫu
      context.SaveChanges();
  }
  ```

---

## GIAI ĐOẠN 4: Bài Tập Mở Rộng 7.3.2 (Thêm thuộc tính `Color`)

### 6. Cập nhật Model Entity `Product`

- [ ] Mở file `src/SportsStore.Domain/Entities/Product.cs`.
- [ ] Thêm thuộc tính `Color` (cho phép NULL):

  ```csharp
  public class Product
  {
      public int ProductID { get; set; }
      public string Name { get; set; } = string.Empty;
      public string Description { get; set; } = string.Empty;
      public decimal Price { get; set; }
      public string Category { get; set; } = string.Empty;
      public string? Color { get; set; }
  }
  ```

---

### 7. Tạo Migration thứ hai (`AddProductColor`)

- [ ] Chạy lệnh tạo migration cập nhật schema:

  ```bash
  dotnet ef migrations add AddProductColor --project src/SportsStore.Infrastructure --startup-project src/SportsStore.WebUI
  ```

- [ ] Quan sát file `..._AddProductColor.cs` kiểm tra câu lệnh `migrationBuilder.AddColumn(...)`.

---

### 8. Áp dụng Migration mới lên CSDL

- [ ] Cập nhật CSDL lần 2:

  ```bash
  dotnet ef database update --project src/SportsStore.Infrastructure --startup-project src/SportsStore.WebUI
  ```

- [ ] Kiểm tra bảng `Products` trong SQL Server đã có thêm cột `Color` (dữ liệu cũ mang giá trị `NULL`).

---

## GIAI ĐOẠN 5: Kiểm tra & Xác thực

- [ ] Biên dịch dự án: `dotnet build`
- [ ] Khởi chạy Web Application: `dotnet run --project src/SportsStore.WebUI`
- [ ] Truy cập trình duyệt và kiểm tra ứng dụng chạy ổn định, hiển thị đầy đủ danh sách sản phẩm.
