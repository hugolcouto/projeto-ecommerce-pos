using System.Net;
using Ecommerce.Application.Common;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Events;
using Ecommerce.Core.Repositories;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Geolocation;
using Ecommerce.Infrastructure.Messaging;
using Microsoft.Extensions.Options;

namespace Ecommerce.Application.Commands.Orders.CreateOrder;

public class CreateOrderCommandHandler(
    IOrderRepository repository,
    IEventPublisher eventPublisher,
    IGeolocationService geolocationService,
    IOptions<GeolocationSettings> geolocationSettings,
    IOrderDomainService orderDomainService,
    ICustomerRepository? customerRepository
) : IHandler<CreateOrderCommand, ResultViewModel<Guid>>
{
    private readonly IOrderRepository _repository = repository;
    private readonly IEventPublisher _eventPublisher = eventPublisher;
    private readonly IGeolocationService _geolocationService = geolocationService;
    private readonly IOptions<GeolocationSettings> _geolocationSettings = geolocationSettings;
    private readonly IOrderDomainService _orderDomainService = orderDomainService;
    readonly ICustomerRepository? _customerRepository = customerRepository;

    public async Task<ResultViewModel<Guid>> HandleAsync(CreateOrderCommand request)
    {
        CustomerAddress? address = await _customerRepository.GetAddress(request.IdCustomer);

        if (address is null)
        {
            return ResultViewModel<Guid>.Error(
                "Address not found",
                HttpStatusCode.NotFound,
                Guid.Empty
            );
        }

        decimal totalShippingCost = await CalculateShipping(request, address.GetFullAddress());

        if (totalShippingCost is -1)
        {
            return ResultViewModel<Guid>.Error(
                "Calculate Error",
                HttpStatusCode.InternalServerError,
                Guid.Empty
            );
        }

        Order order = new(
            request.IdCustomer,
            request.IdDeliveryAddress,
            totalShippingCost,
            100.0m,
            [.. request.Items.Select(i => new OrderItem(i.IdProduct, 5, 100m))]
        );

        order.SetShippingCost(totalShippingCost);

        await _repository.CreateAsync(order);

        OrderCreatedEvent @event = new(order.Id);

        await _eventPublisher.PublisherAsync(@event);

        return ResultViewModel<Guid>.Success(order.Id);
    }

    private async Task<decimal> CalculateShipping(CreateOrderCommand request, string destination)
    {
        int distanceInKm = await _geolocationService.GetDistance(
            _geolocationSettings.Value.Origin,
            destination
        );

        List<OrderItem> items =
        [
            .. request.Items.Select(i => new OrderItem(i.IdProduct, i.Quantity, 0)),
        ];

        decimal totalShippingCost = _orderDomainService.CalculateShippingCost(distanceInKm, items);

        return totalShippingCost;
    }
}
