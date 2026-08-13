using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;

namespace Ecommerce.Core.Services;

public class OrderDomainService(IProductRepository productRepository) : IOrderDomainService
{
    private const decimal _pricePerKm = 30;
    private const decimal _pricePerUnit = 2.5m;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<decimal> CalculateProductOrderTotal(List<OrderItem> items)
    {
        decimal total = 0;

        foreach (OrderItem item in items)
            total += item.Price * item.Quantity;

        return total;
    }

    public decimal CalculateShippingCost(int distanceInKm, List<OrderItem> items)
    {
        var totalPriceKm = _pricePerKm * distanceInKm;

        var totalUnits = items.Sum(i => i.Quantity);
        var totalPriceUnits = _pricePerUnit * totalUnits;

        var total = totalPriceUnits + totalPriceKm;

        return total;
    }

    public async Task UpdateProductPrices(Order order)
    {
        foreach (OrderItem item in order.Items)
        {
            Product? product =
                await _productRepository.GetById(item.IdProduct)
                ?? throw new InvalidOperationException("Invalid product");

            item.Price += product.Price * item.Quantity;
        }
    }
}
