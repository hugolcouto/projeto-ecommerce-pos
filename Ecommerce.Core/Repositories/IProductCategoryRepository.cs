using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Repositories;

public interface IProductCategoryRepository
{
    Task<Guid> Create(ProductCategory productCategory);
}
