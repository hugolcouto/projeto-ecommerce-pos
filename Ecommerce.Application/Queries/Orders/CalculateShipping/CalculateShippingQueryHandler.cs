using System.Net;
using Ecommerce.Application.Common;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Geolocation;
using Microsoft.Extensions.Options;

namespace Ecommerce.Application;

public class CalculateShippingQueryHandler
    : IHandler<CalculateShippingQuery, ResultViewModel<decimal>>
{
    private readonly IGeolocationService _geolocationService;
    private readonly IOrderDomainService _orderDomainService;
    private readonly GeolocationSettings _geolocationSettings;

    public CalculateShippingQueryHandler(
        IGeolocationService geolocationService,
        IOrderDomainService orderDomainService,
        IOptions<GeolocationSettings> options
    )
    {
        _geolocationService = geolocationService;
        _orderDomainService = orderDomainService;
        _geolocationSettings = options.Value;
    }

    public async Task<ResultViewModel<decimal>> HandleAsync(CalculateShippingQuery request)
    {
        int distanceInKm = await _geolocationService.GetDistance(
            _geolocationSettings.Origin,
            request.ZipCode
        );

        List<OrderItem> items =
        [
            .. request.Items.Select(i => new OrderItem(i.IdProduct, i.Quantity, 0)),
        ];

        decimal totalShippingCost = _orderDomainService.CalculateShippingCost(distanceInKm, items);

        if (totalShippingCost is -1)
        {
            return ResultViewModel<decimal>.Error(
                "Calculate Error",
                HttpStatusCode.InternalServerError,
                0
            );
        }

        return ResultViewModel<decimal>.Success(totalShippingCost);
    }
}
