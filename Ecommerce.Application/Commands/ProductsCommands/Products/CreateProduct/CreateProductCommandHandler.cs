using Ecommerce.Application.Common;

namespace Ecommerce.Application.Commands.ProductsCommands.Products.CreateProduct;

public class CreateProductCommandHandler : IHandler<CreateProductCommand, ResultViewModel<Guid>>
{
    public Task<ResultViewModel<Guid>> HandleAsync(CreateProductCommand request)
    {
        throw new NotImplementedException();
    }
}