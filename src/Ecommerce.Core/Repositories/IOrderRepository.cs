using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Repositories;

public interface IOrderRepository
{
    Task<Guid> CreateAsync(Order order);
    Task<Order?> GetByIdAsync(Guid id);
    Task UpdateAsync(Order order);
}
