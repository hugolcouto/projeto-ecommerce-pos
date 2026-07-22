using Azure.Storage.Blobs;
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
            services
                .AddData(configuration)
                .AddRepositories()
                .AddMessaging(configuration)
                .AddStorage(configuration);

            return services;
        }

        private IServiceCollection AddRepositories()
        {
            services
                .AddScoped<ICustomerRepository, CustomerRepository>()
                .AddScoped<IOrderRepository, OrderRepository>()
                .AddScoped<IProductCategoryRepository, ProductCategoryRepository>()
                .AddScoped<IProductRepository, ProductRepository>()
                .AddScoped<IStorageService, StorageService>();

            return services;
        }

        private IServiceCollection AddData(IConfiguration configuration)
        {
            string dbConnectionString =
                configuration.GetConnectionString("SqlConnectionString")
                ?? throw new InvalidOperationException(
                    "Connection string 'SqlConnectionString' not found."
                );
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

        private IServiceCollection AddStorage(IConfiguration configuration)
        {
            string connectionString =
                configuration.GetConnectionString("BlobStorage")
                ?? throw new InvalidOperationException("ConnectionString 'BlobStorage' not found");

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            services.AddSingleton(blobServiceClient);

            services.AddScoped<IStorageService, StorageService>();

            return services;
        }
    }
}
