using Ecommerce.Application.Common;
using Ecommerce.Infrastructure.Caching;

namespace Ecommerce.Application.Commands.ShoppingCarts.CreateOrUpdateShoppingCart;

public class CreateOrUpdateShoppingCartCommandHandler(ICacheService cacheService)
        : IHandler<CreateOrUpdateShoppingCartCommand, ResultViewModel<bool>>
{
    private readonly ICacheService _cacheService = cacheService;

    public async Task<ResultViewModel<bool>> HandleAsync(CreateOrUpdateShoppingCartCommand request)
    {
        string cacheKey = request.IdCustomer.ToString();

        await _cacheService.SetAsync(cacheKey, request.Items);

        return ResultViewModel<bool>.Success(true);
    }
}
