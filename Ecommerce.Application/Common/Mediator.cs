using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Application.Common;

public class Mediator(IServiceScopeFactory factory) : IMediator
{
    public async Task<TResponse> DispatchAsync<TRequest, TResponse>(TRequest request)
    {
        var scope = factory.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IHandler<TRequest, TResponse>>();

        return await handler.HandleAsync(request);
    }
}