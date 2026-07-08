using System;
using System.Text;
using System.Text.Json;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Events;
using Ecommerce.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ecommerce.Infrastructure.Messaging.Consumers;

public class OrderCreatedEventConsumer : BackgroundService
{
    private readonly RabbitMqSettings _settings;
    private readonly IServiceProvider _serviceProvider;
    private IConnection _connection;
    private IChannel _channel;

    public OrderCreatedEventConsumer(RabbitMqSettings settings, IServiceProvider serviceProvider)
    {
        _settings = settings;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await InitializeRabbitMQAsync();

        AsyncEventingBasicConsumer consumer = new(_channel);

        consumer.ReceivedAsync += async (model, eventargs) =>
        {
            try
            {
                byte[] body = eventargs.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                OrderCreatedEvent? @event = JsonSerializer.Deserialize<OrderCreatedEvent>(message);

                Console.WriteLine(
                    $"[Consumer] Received OrderCreatedEvent with id {@event.IdOrder}"
                );

                IServiceScope? scope = _serviceProvider.CreateScope();
                IOrderRepository? repository =
                    scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                Order? order = await repository.GetByIdAsync(@event.IdOrder);

                if (order is null)
                {
                    Console.WriteLine($"[Consumer] Order with id {@event.IdOrder} does not exists");
                    return;
                }

                order.MarkAsConfirmed();

                await repository.UpdateAsync(order);

                Console.WriteLine($"[Consumer] Order with ID {@event.IdOrder} updated");

                await _channel.BasicAckAsync(
                    eventargs.DeliveryTag,
                    false,
                    cancellationToken: stoppingToken
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Consumer] Exception: {ex.Message}");

                await _channel.BasicNackAsync(
                    eventargs.DeliveryTag,
                    false,
                    true,
                    cancellationToken: stoppingToken
                );
            }
        };

        await _channel.BasicConsumeAsync(
            queue: _settings.QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken
        );
    }

    private async Task InitializeRabbitMQAsync()
    {
        ConnectionFactory connectionFactory = new()
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            Password = _settings.Password,
            UserName = _settings.UserName,
        };

        _connection = connectionFactory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;

        await _channel.ExchangeDeclareAsync(
            exchange: _settings.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false
        );

        await _channel.QueueDeclareAsync(
            queue: _settings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        await _channel.QueueBindAsync(
            queue: _settings.QueueName,
            exchange: _settings.ExchangeName,
            routingKey: "ordercreated"
        );

        Console.WriteLine(
            $"[Consumer]: RabbitMQInitialized - Exchange: {_settings.ExchangeName} - Queue: {_settings.QueueName}"
        );
    }
}
