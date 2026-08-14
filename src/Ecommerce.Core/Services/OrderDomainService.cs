using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;

namespace Ecommerce.Core.Services;

public class OrderDomainService(IProductRepository productRepository) : IOrderDomainService
{
    private const decimal _pricePerKm = 30m;
    private const decimal _pricePerUnit = 2.5m;
    private const int _maximumAllowedDistanceKm = 250;
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
        if (distanceInKm > _maximumAllowedDistanceKm)
        {
            throw new ArgumentOutOfRangeException(
                nameof(distanceInKm),
                "Above maximum accepted value"
            );
        }

        if (items.Count == 0)
            throw new InvalidOperationException("No items found in cart");

        decimal totalPriceKm = distanceInKm == 0 ? _pricePerKm : _pricePerKm * distanceInKm;

        int totalUnits = items.Sum(i => i.Quantity);
        decimal totalPriceUnits = _pricePerUnit * totalUnits;

        decimal total = totalPriceUnits + totalPriceKm;

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
