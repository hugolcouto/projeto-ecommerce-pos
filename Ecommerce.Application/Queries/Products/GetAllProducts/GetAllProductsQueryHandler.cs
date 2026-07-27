using Ecommerce.Application.Common;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;
using Ecommerce.Infrastructure.Caching;

namespace Ecommerce.Application.Queries.Products.GetAllProducts;

public class GetAllProductsQueryHandler
    : IHandler<GetAllProductsQuery, ResultViewModel<List<GetAllProductQueryItemViewModel>>>
{
    private readonly IProductRepository _productRepository;
    private readonly ICacheService _cacheService;
    private const string CacheKey = "products:all";

    public GetAllProductsQueryHandler(
        IProductRepository productRepository,
        ICacheService cacheService
    )
    {
        _productRepository = productRepository;
        _cacheService = cacheService;
    }

    public async Task<ResultViewModel<List<GetAllProductQueryItemViewModel>>> HandleAsync(
        GetAllProductsQuery request
    )
    {
        List<GetAllProductQueryItemViewModel>? cachedProducts = await _cacheService.GetAsync<
            List<GetAllProductQueryItemViewModel>
        >(CacheKey);

        if (cachedProducts is not null)
            return ResultViewModel<List<GetAllProductQueryItemViewModel>>.Success(cachedProducts);

        List<Product> products = await _productRepository.GetAll();

        List<GetAllProductQueryItemViewModel> productsViewModel =
        [
            .. products.Select(p => new GetAllProductQueryItemViewModel()
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
            }),
        ];

        await _cacheService.SetAsync(CacheKey, productsViewModel);

        return ResultViewModel<List<GetAllProductQueryItemViewModel>>.Success(productsViewModel);
    }
}
