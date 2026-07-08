using Ecommerce.Core.Events;
using Ecommerce.Core.Repositories;
using Ecommerce.Infrastructure.Messaging;
using Ecommerce.Infrastructure.Messaging.Consumers;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure;

public static class InfrastructureModule
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services.AddData(configuration).AddRepositories().AddMessaging(configuration);

            return services;
        }

        private IServiceCollection AddRepositories()
        {
            services
                .AddScoped<ICustomerRepository, CustomerRepository>()
                .AddScoped<IOrderRepository, OrderRepository>()
                .AddScoped<IProductCategoryRepository, ProductCategoryRepository>()
                .AddScoped<IProductRepository, ProductRepository>();

            return services;
        }

        private IServiceCollection AddData(IConfiguration configuration)
        {
            string dbConnectionString = configuration.GetConnectionString("SqlConnectionString")!;
            services.AddDbContext<EcommerceDbContext>(options =>
                options.UseNpgsql(dbConnectionString)
            );

            return services;
        }

        private IServiceCollection AddMessaging(IConfiguration configuration)
        {
            RabbitMqSettings rabbitMqSettings = new();

            configuration.GetSection("RabbitMQ").Bind(rabbitMqSettings);

            services.AddSingleton(rabbitMqSettings);
            services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
            services.AddHostedService<OrderCreatedEventConsumer>();

            return services;
        }
    }
}
