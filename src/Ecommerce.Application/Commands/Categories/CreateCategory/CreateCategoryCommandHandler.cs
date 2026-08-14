using Ecommerce.Application.Common;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;

namespace Ecommerce.Application.Commands.ProductsCommands.Categories.CreateCategory;

public class CreateCategoryCommandHandler(IMediator mediator, IProductCategoryRepository repository)
    : IHandler<CreateCategoryCommand, ResultViewModel<Guid>>
{
    private readonly IMediator _mediator = mediator;
    private readonly IProductCategoryRepository _repository = repository;

    public async Task<ResultViewModel<Guid>> HandleAsync(CreateCategoryCommand request)
    {
        ProductCategory category = new(request.Title, request.Subcategory);

        await _repository.Create(category);

        return ResultViewModel<Guid>.Success(category.Id);
    }
}
