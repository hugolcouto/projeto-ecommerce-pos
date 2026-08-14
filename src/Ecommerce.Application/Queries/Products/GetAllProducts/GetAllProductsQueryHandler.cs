using Ecommerce.Application.Common;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;
using Ecommerce.Infrastructure.Caching;

namespace Ecommerce.Application.Queries.Products.GetAllProducts;

public class GetAllProductsQueryHandler(
    IProductRepository productRepository,
    ICacheService cacheService
) : IHandler<GetAllProductsQuery, ResultViewModel<List<GetAllProductsItemViewModel>>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICacheService _cacheService = cacheService;
    private const string CacheKey = "products:all";

    public async Task<ResultViewModel<List<GetAllProductsItemViewModel>>> HandleAsync(
        GetAllProductsQuery request
    )
    {
        List<GetAllProductsItemViewModel>? cachedProducts = await _cacheService.GetAsync<
            List<GetAllProductsItemViewModel>
        >(CacheKey);

        if (cachedProducts is not null)
            return ResultViewModel<List<GetAllProductsItemViewModel>>.Success(cachedProducts);

        List<Product> products = await _productRepository.GetAll();

        List<GetAllProductsItemViewModel> productsViewModel = products.ConvertAll(
            p => new GetAllProductsItemViewModel()
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
            }
        );

        await _cacheService.SetAsync(CacheKey, productsViewModel);

        return ResultViewModel<List<GetAllProductsItemViewModel>>.Success(productsViewModel);
    }
}
