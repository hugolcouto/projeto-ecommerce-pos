using Ecommerce.Application.Common;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;

namespace Ecommerce.Application.Commands.ProductsCommands.Customers.CreateCostumerAddresses;

public class CreateCustomerAddressCommandHandler
    : IHandler<CreateCustomerAddressCommand, ResultViewModel<Guid>>
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerAddressCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultViewModel<Guid>> HandleAsync(CreateCustomerAddressCommand request)
    {
        CustomerAddress address = new(
            request.IdCustomer,
            request.RecipientName,
            request.AddressLine1,
            request.AddressLine2,
            request.ZipCode,
            request.Street,
            request.District,
            request.City,
            request.State
        );

        await _repository.CreateAddress(address);

        return ResultViewModel<Guid>.Success(address.Id);
    }
}
