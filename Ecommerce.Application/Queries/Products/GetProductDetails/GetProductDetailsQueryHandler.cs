using System.Net;
using Ecommerce.Application.Common;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;
using Ecommerce.Infrastructure.Caching;
using Ecommerce.Infrastructure.Repositories;

namespace Ecommerce.Application.Queries.Products.GetProductDetails;

public class GetProductDetailsQueryHandler
    : IHandler<GetProductDetailsQuery, ResultViewModel<ProductDetailsViewModel>>
{
    private readonly IProductRepository _productRepository;
    private readonly ICacheService _cacheService;
    private const string CacheKeyPrefix = "product:";

    public GetProductDetailsQueryHandler(
        IProductRepository productRepository,
        ICacheService cacheService
    )
    {
        _productRepository = productRepository;
        _cacheService = cacheService;
    }

    public async Task<ResultViewModel<ProductDetailsViewModel>> HandleAsync(
        GetProductDetailsQuery request
    )
    {
        string cacheKey = $"{CacheKeyPrefix}{request.IdProduct}";

        ProductDetailsViewModel? cachedProduct =
            await _cacheService.GetAsync<ProductDetailsViewModel>(cacheKey);

        if (cachedProduct is not null)
            return ResultViewModel<ProductDetailsViewModel>.Success(cachedProduct);

        Product? product = await _productRepository.GetById(request.IdProduct);

        if (product is null)
            return ResultViewModel<ProductDetailsViewModel>.Error( 
                "Product not found",
                HttpStatusCode.NotFound,
                null
            );

        ProductDetailsViewModel productDetailsViewModel = ProductDetailsViewModel.FromEntity(product);

        await _cacheService.SetAsync(cacheKey, productDetailsViewModel);

        return ResultViewModel<ProductDetailsViewModel>.Success(productDetailsViewModel);
    }
}
