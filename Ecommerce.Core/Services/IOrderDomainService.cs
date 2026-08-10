using System;
using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Services;

public interface IOrderDomainService
{
    decimal CalculateShippingCost(int distanceInKm, List<OrderItem> items);
}
