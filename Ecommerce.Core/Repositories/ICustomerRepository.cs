using Ecommerce.Core.Entities;

namespace Ecommerce.Core.Repositories;

public interface ICustomerRepository
{
    Task<Guid> Create(Customer customer);
    Task<Guid> CreateAddress(CustomerAddress address);
}
