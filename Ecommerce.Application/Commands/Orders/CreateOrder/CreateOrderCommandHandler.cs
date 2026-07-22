using Ecommerce.Application.Common;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Events;
using Ecommerce.Core.Repositories;
using Ecommerce.Infrastructure.Messaging;

namespace Ecommerce.Application.Commands.Orders.CreateOrder;

public class CreateOrderCommandHandler : IHandler<CreateOrderCommand, ResultViewModel<Guid>>
{
    private readonly IOrderRepository _repository;
    private readonly IEventPublisher _eventPublisher;

    public CreateOrderCommandHandler(IOrderRepository repository, IEventPublisher eventPublisher)
    {
        _repository = repository;
        _eventPublisher = eventPublisher;
    }

    public async Task<ResultViewModel<Guid>> HandleAsync(CreateOrderCommand request)
    {
        Order order = new(
            request.IdCustomer,
            request.IdDeliveryAddress,
            10.0m,
            100.0m,
            [.. request.Items.Select(i => new OrderItem(i.IdProduct, 5, 100m))]
        );

        await _repository.CreateAsync(order);

        OrderCreatedEvent @event = new(order.Id);

        await _eventPublisher.PublisherAsync(@event);

        return ResultViewModel<Guid>.Success(order.Id);
    }
}
