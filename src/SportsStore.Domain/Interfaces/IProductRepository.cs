using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Interfaces;

public interface IProductRepository
{
    // Yêu cầu thủ kho (Infrastructure) phải trả về danh sách sản phẩm
    IQueryable<Product> Products { get; }
}