using Ecommerce.Application.Common;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;
using Ecommerce.Infrastructure.Caching;

namespace Ecommerce.Application.Commands.ProductsCommands.Products.CreateProduct;

public class CreateProductCommandHandler : IHandler<CreateProductCommand, ResultViewModel<Guid>>
{
    private readonly IProductRepository _repository;
    private readonly ICacheService _cacheService;

    public CreateProductCommandHandler(IProductRepository repository, ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
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

        await _cacheService.RemoveAsync("products:all");

        return ResultViewModel<Guid>.Success(product.Id);
    }
}
