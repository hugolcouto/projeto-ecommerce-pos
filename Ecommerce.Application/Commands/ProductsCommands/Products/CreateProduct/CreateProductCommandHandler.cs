using Ecommerce.Application.Common;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;

namespace Ecommerce.Application.Commands.ProductsCommands.Products.CreateProduct;

public class CreateProductCommandHandler : IHandler<CreateProductCommand, ResultViewModel<Guid>>
{
    private readonly IProductRepository _repository;

    public CreateProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultViewModel<Guid>> HandleAsync(CreateProductCommand request)
    {
        Product product = new(
            request.Title,
            request.Description,
            request.Price,
            request.Brand,
            request.Quantity,
            request.IdCategory
        );

        await _repository.Create(product);

        return ResultViewModel<Guid>.Success(product.Id);
    }
}
