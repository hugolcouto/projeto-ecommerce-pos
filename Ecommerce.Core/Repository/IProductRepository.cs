using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Repository;

public interface IProductRepository
{
    Task<Guid> Create(Product product);
    Task<List<Product>> GetAll();
    Task<Product> GetById();
}