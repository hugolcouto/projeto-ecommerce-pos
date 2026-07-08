using Ecommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ecommerce.Infrastructure.Persistence;

public class EcommerceDbContext : DbContext
{
    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
        : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerAddress> CustomerAddresses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderItemReview> OrderProductReviews { get; set; }
    public DbSet<OrderUpdate> OrderUpdates { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTimeOffset))
                {
                    property.SetValueConverter(
                        new ValueConverter<DateTimeOffset, DateTimeOffset>(
                            v => v.ToUniversalTime(),
                            v => v.ToLocalTime()
                        )
                    );
                }
            }
        }

        modelBuilder.Entity<Customer>(e =>
        {
            e.HasKey(c => c.Id);

            e.HasMany(c => c.Addresses)
                .WithOne()
                .HasForeignKey(a => a.IdCustomer)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.IdCustomer)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(c => c.Reviews)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.IdCustomer)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CustomerAddress>(e =>
        {
            e.HasKey(c => c.Id);
        });

        modelBuilder.Entity<Order>(e =>
        {
            e.HasKey(o => o.Id);

            e.HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey(o => o.IdOrder)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(o => o.Updates)
                .WithOne()
                .HasForeignKey(o => o.IdOrder)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderItem>(e =>
        {
            e.HasKey(o => o.Id);

            e.HasOne(oi => oi.Review)
                .WithOne(opr => opr.OrderItem)
                .HasForeignKey<OrderItemReview>(opr => opr.IdOrderItem)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.IdProduct)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderItemReview>(e =>
        {
            e.HasKey(o => o.Id);
        });

        modelBuilder.Entity<OrderUpdate>(e =>
        {
            e.HasKey(o => o.Id);
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(p => p.Id);

            e.HasMany(p => p.Images)
                .WithOne()
                .HasForeignKey(pi => pi.IdProduct)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProductCategory>(e =>
        {
            e.HasKey(pc => pc.Id);

            e.HasMany(pc => pc.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.IdCategory)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProductImage>(e =>
        {
            e.HasKey(pi => pi.Id);
        });

        base.OnModelCreating(modelBuilder);
    }
}
