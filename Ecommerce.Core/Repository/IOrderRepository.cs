using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Repository;

public interface IOrderRepository
{
    Task<Guid> Create(Order order);
}