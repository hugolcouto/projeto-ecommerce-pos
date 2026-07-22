using Ecommerce.Application.Common;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;

namespace Ecommerce.Application.Commands.ProductsCommands.Customers.CreateCostumer;

public class CreateCustomerCommandHandler : IHandler<CreateCustomerCommand, ResultViewModel<Guid>>
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultViewModel<Guid>> HandleAsync(CreateCustomerCommand request)
    {
        Customer customer = new(
            request.FullName,
            request.Email,
            request.PhoneNumber,
            request.Document,
            request.BirthDate
        );

        await _repository.Create(customer);

        return ResultViewModel<Guid>.Success(customer.Id);
    }
}
