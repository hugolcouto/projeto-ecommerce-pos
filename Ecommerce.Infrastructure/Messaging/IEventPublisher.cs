using System;

namespace Ecommerce.Infrastructure.Messaging;

public interface IEventPublisher
{
    Task PublisherAsync<TEvent>(TEvent @event)
        where TEvent : class;
}
