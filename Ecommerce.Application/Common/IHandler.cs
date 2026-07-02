namespace Ecommerce.Application.Common;

public interface IHandler<TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request);
}