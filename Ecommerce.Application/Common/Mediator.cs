using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Application.Common;

public class Mediator : IMediator
{
    private readonly IServiceScopeFactory _factory;

    public Mediator(IServiceScopeFactory factory)
    {
        _factory = factory;
    }

    public async Task<TResponse> DispatchAsync<TRequest, TResponse>(TRequest request)
    {
        var scope = _factory.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IHandler<TRequest, TResponse>>();

        return await handler.HandleAsync(request);
    }
}