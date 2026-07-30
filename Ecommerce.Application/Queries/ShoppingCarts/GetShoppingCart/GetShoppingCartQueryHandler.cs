using System.Net;
using Ecommerce.Application.Common;
using Ecommerce.Application.Common.ShoppingCart;
using Ecommerce.Infrastructure.Caching;

namespace Ecommerce.Application.Queries.ShoppingCarts;

public class GetShoppingCartQueryHandler
    : IHandler<GetShoppingCartQuery, ResultViewModel<List<ProductItemShoppingCartModel>>>
{
    private readonly ICacheService _cacheService;

    public GetShoppingCartQueryHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    async Task<ResultViewModel<List<ProductItemShoppingCartModel>>> IHandler<
        GetShoppingCartQuery,
        ResultViewModel<List<ProductItemShoppingCartModel>>
    >.HandleAsync(GetShoppingCartQuery request)
    {
        string cacheKey = request.IdCustomer.ToString();

        List<ProductItemShoppingCartModel>? cacheResult = await _cacheService.GetAsync<
            List<ProductItemShoppingCartModel>
        >(cacheKey);

        if (cacheResult is null)
            return ResultViewModel<List<ProductItemShoppingCartModel>>.Error(
                "Entry not found",
                HttpStatusCode.NotFound,
                null
            );

        return ResultViewModel<List<ProductItemShoppingCartModel>>.Success(cacheResult);
    }
}
