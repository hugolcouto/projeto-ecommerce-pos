using System;
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Services;

public interface IOrderDomainService
{
    decimal CalculateShippingCost(int distanceInKm, List<OrderItem> items);
    Task<decimal> CalculateProductOrderTotal(List<OrderItem> items);
    Task UpdateProductPrices(Order order);
}
