using System;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;
using Ecommerce.Infrastructure.Persistence;

namespace Ecommerce.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly EcommerceDbContext _context;

    public CustomerRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Create(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        return customer.Id;
    }

    public async Task<Guid> CreateAddress(CustomerAddress address)
    {
        await _context.AddAsync(address);
        await _context.SaveChangesAsync();

        return address.Id;
    }
}
