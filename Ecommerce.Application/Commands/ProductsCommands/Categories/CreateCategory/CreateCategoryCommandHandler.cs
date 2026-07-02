using Ecommerce.Application.Common;

namespace Ecommerce.Application.Commands.ProductsCommands.Categories.CreateCategory;

public class CreateCategoryCommandHandler : IHandler<CreateCategoryCommand, ResultViewModel<Guid>>
{
    public Task<ResultViewModel<Guid>> HandleAsync(CreateCategoryCommand request)
    {
        throw new NotImplementedException();
    }
}