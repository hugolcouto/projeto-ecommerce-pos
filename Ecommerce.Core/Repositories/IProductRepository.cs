using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Repositories;

public interface IProductRepository
{
    Task<Guid> Create(Product product);
    Task<List<Product>> GetAll();
    Task<Product?> GetById(Guid id);
    Task CreateImage(ProductImage productImage);
    Task<ProductImage?> GetImageById(Guid id);
}
