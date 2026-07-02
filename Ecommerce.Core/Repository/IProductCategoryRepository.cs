using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Repository;

interface IProductCategoryRepository
{
    Task<Guid> Create(ProductCategory productCategory);
}