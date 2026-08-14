using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;
using Ecommerce.Infrastructure.Persistence;

namespace Ecommerce.Infrastructure.Repositories;

public class ProductCategoryRepository(EcommerceDbContext context) : IProductCategoryRepository
{
    private readonly EcommerceDbContext _context = context;

    public async Task<Guid> Create(ProductCategory productCategory)
    {
        await _context.ProductCategories.AddAsync(productCategory);
        await _context.SaveChangesAsync();

        return productCategory.Id;
    }
}
