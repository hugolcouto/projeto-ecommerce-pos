using System;

namespace Ecommerce.Core.Events;

public class OrderCreatedEvent(Guid idOrder)
{
    public Guid IdOrder { get; private set; } = idOrder;
}
