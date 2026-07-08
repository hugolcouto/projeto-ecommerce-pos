using System;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;

namespace Ecommerce.Infrastructure.Messaging;

public class RabbitMqEventPublisher : IEventPublisher
{
    private readonly RabbitMqSettings _settings;
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    public RabbitMqEventPublisher(RabbitMqSettings settings)
    {
        _settings = settings;
        ConnectionFactory connectionFactory = new()
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            Password = _settings.Password,
            UserName = _settings.UserName,
        };

        _connection = connectionFactory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;

        _channel
            .ExchangeDeclareAsync(
                exchange: _settings.ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false
            )
            .GetAwaiter()
            .GetResult();
    }

    public async Task PublisherAsync<TEvent>(TEvent @event)
        where TEvent : class
    {
        string eventName = typeof(TEvent).Name;
        string routingKey = eventName.ToLower().Replace("event", "");

        string message = JsonSerializer.Serialize(@event);
        byte[] body = Encoding.UTF8.GetBytes(message);

        await _channel.BasicPublishAsync(
            exchange: _settings.ExchangeName,
            routingKey: routingKey,
            body: body
        );

        Console.WriteLine(
            $"[Publisher] published {eventName} to exchange {_settings.ExchangeName} with routing key {routingKey}"
        );
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}
