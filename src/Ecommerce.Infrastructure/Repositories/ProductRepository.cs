using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class ProductRepository(EcommerceDbContext context) : IProductRepository
{
    private readonly EcommerceDbContext _context = context;

    public async Task<Guid> Create(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return product.Id;
    }

    public async Task CreateImage(ProductImage productImage)
    {
        await _context.ProductImages.AddAsync(productImage);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetAll()
    {
        return await _context.Products.Where(p => !p.IsDeleted).ToListAsync();
    }

    public async Task<Product?> GetById(Guid id)
    {
        return await _context
            .Products.Include(p => p.Category)
            .Include(p => p.Reviews)
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<ProductImage?> GetImageById(Guid id)
    {
        ProductImage? image = await _context.ProductImages.SingleOrDefaultAsync(i => i.Id == id);

        return image;
    }
}
