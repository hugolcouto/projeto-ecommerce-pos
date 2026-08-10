using Ecommerce.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Core;

public static class CoreModule
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddCore()
        {
            services.AddDomainService();

            return services;
        }

        private IServiceCollection AddDomainService()
        {
            services.AddScoped<IOrderDomainService, OrderDomainService>();

            return services;
        }
    }
}
