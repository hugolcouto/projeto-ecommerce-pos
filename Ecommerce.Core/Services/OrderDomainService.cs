using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Services;

public class OrderDomainService : IOrderDomainService
{
    private const decimal _pricePerKm = 30;
    private const decimal _pricePerUnit = 2.5m;

    public decimal CalculateShippingCost(int distanceInKm, List<OrderItem> items)
    {
        var totalPriceKm = _pricePerKm * distanceInKm;

        var totalUnits = items.Sum(i => i.Quantity);
        var totalPriceUnits = _pricePerUnit * totalUnits;

        var total = totalPriceUnits + totalPriceKm;

        return total;
    }
}
