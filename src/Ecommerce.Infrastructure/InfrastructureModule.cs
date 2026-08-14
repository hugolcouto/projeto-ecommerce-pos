using Azure.Storage.Blobs;
using Ecommerce.Core.Repositories;
using Ecommerce.Infrastructure.BackgroundJobs;
using Ecommerce.Infrastructure.Caching;
using Ecommerce.Infrastructure.Geolocation;
using Ecommerce.Infrastructure.Geolocation.GoogleMaps;
using Ecommerce.Infrastructure.Messaging;
using Ecommerce.Infrastructure.Messaging.Consumers;
using Ecommerce.Infrastructure.Payment;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repositories;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
                .AddStorage(configuration)
                .AddCache(configuration)
                .AddGeolocation(configuration)
                .AddHangfireServices(configuration)
                .AddPaymentService(configuration);

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
                ?? throw new InvalidOperationException("DB Connection strign not configured");
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

            BlobServiceClient blobServiceClient = new(connectionString);

            services.AddSingleton(blobServiceClient);

            services.AddScoped<IStorageService, StorageService>();

            return services;
        }

        private IServiceCollection AddCache(IConfiguration configuration)
        {
            string cachingProvider =
                configuration.GetValue<string>("Caching:Provider") ?? string.Empty;

            if (cachingProvider == "Redis")
            {
                string connectionString =
                    configuration.GetValue<string>("Caching:Redis:ConnectionString")
                    ?? throw new InvalidOperationException(
                        "Redis connection string not configured"
                    );

                string instanceName =
                    configuration.GetValue<string>("Caching:Redis:InstanceName")
                    ?? throw new InvalidOperationException("Redis instance name not configured");

                services.AddStackExchangeRedisCache(o =>
                {
                    o.Configuration = connectionString;
                    o.InstanceName = instanceName;
                });

                services.AddScoped<ICacheService, RedisCacheService>();
            }
            else
            {
                services.AddMemoryCache();
                services.AddScoped<ICacheService, MemoryCacheService>();
            }

            return services;
        }

        private IServiceCollection AddGeolocation(IConfiguration configuration)
        {
            string geolocationProvider =
                configuration.GetValue<string>("Geolocation:Provider") ?? string.Empty;

            if (geolocationProvider is "GoogleMaps")
            {
                services.Configure<GeolocationSettings>(
                    configuration.GetSection("Geolocation:GoogleMaps")
                );

                services.AddScoped<IGeolocationService, GeolocationService>();
            }

            return services;
        }

        private IServiceCollection AddHangfireServices(IConfiguration configuration)
        {
            string connectionString =
                configuration.GetConnectionString("SqlConnectionString")
                ?? throw new InvalidOperationException("DB Connection strign not configured");

            services.AddHangfire(config =>
            {
                config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(o => o.UseNpgsqlConnection(connectionString));
            });

            services.AddHangfireServer();

            return services;
        }

        private IServiceCollection AddPaymentService(IConfiguration configuration)
        {
            string? paymentProvider =
                configuration.GetValue<string>("Payment:Provider") ?? string.Empty;

            if (paymentProvider == "Stripe")
            {
                services.AddScoped<IPaymentService, StripePaymentService>();
                services.Configure<PaymentSettings>(configuration.GetSection("Payment:Stripe"));
            }

            return services;
        }
    }
}
