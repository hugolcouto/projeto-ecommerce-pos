using System.Text;
using System.Text.Json;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Events;
using Ecommerce.Core.Repositories;
using Ecommerce.Infrastructure.Payment;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ecommerce.Infrastructure.Messaging.Consumers;

public class OrderCreatedEventConsumer(RabbitMqSettings settings, IServiceProvider serviceProvider)
    : BackgroundService
{
    private readonly RabbitMqSettings _settings = settings;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private IConnection _connection;
    private IChannel _channel;

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
                IOrderRepository? orderRepository =
                    scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                Order? order = await orderRepository.GetByIdAsync(@event.IdOrder);

                if (order is null)
                {
                    Console.WriteLine($"[Consumer] Order with id {@event.IdOrder} does not exists");
                    return;
                }

                IPaymentService paymentService =
                    scope.ServiceProvider.GetRequiredService<IPaymentService>();

                ICustomerRepository customerRepository =
                    scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

                Customer? customer = await customerRepository.GetById(order.IdCustomer);

                PaymentCustomerModel customerPaymentModel = new()
                {
                    Email = customer.Email,
                    FullName = customer.FullName,
                    PhoneNumber = customer.PhoneNumber,
                };

                string? customerPaymentId;

                if (customer.IdExternalPayment is not null)
                {
                    customerPaymentId = customer.IdExternalPayment;
                }
                else
                {
                    customerPaymentId = await paymentService.CreateCustomerAsync(
                        customerPaymentModel
                    );

                    customer.IdExternalPayment = customerPaymentId;

                    await customerRepository.Update(customer);
                }

                PaymentOrderModel paymentOrderModel = new()
                {
                    IdExternalCustomer = customerPaymentId,
                    Items = order.Items.ConvertAll(oi => new PaymentOrderItemModel
                    {
                        Name = oi.Product.Title,
                        Price = oi.Price,
                        Quantity = oi.Quantity,
                    }),
                };

                PaymentOrderResponseModel paymentResult = await paymentService.CreateOrderAsync(
                    paymentOrderModel
                );

                order.MarkAsPaymentPending();

                order.IdExternalOrder = paymentResult.Id;
                order.PaymentUrl = paymentResult.Url;
                await orderRepository.UpdateAsync(order);

                // TODO: SignalR Para retorno do link de pagamento

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
