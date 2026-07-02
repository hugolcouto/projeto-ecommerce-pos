using Ecommerce.Application.Common;

namespace Ecommerce.Application.Queries.Products.GetAllProducts;

public class GetAllProductsQueryHandler : IHandler<GetAllProductsQuery, ResultViewModel<List<GetAllProductQueryItemViewModel>>>
{
    public Task<ResultViewModel<List<GetAllProductQueryItemViewModel>>> HandleAsync(GetAllProductsQuery request)
    {
        throw new NotImplementedException();
    }
}