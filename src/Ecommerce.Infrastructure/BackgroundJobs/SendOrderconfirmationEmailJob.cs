using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.BackgroundJobs;

public class SendOrderconfirmationEmailJob(
    IConfiguration configuration,
    ILogger<SendOrderconfirmationEmailJob> logger
)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<SendOrderconfirmationEmailJob> _logger = logger;

    public async Task ExecuteAsync(Guid orderId, string customerEmail)
    {
        _logger.LogInformation($"Sending email for order {orderId} to {customerEmail}");

        await Task.Delay(3000);

        _logger.LogInformation($"Email send successfully to {customerEmail}");
    }
}
