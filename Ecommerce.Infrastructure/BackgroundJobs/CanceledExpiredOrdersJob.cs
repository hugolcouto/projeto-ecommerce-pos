using Ecommerce.Core.Entities;
using Ecommerce.Core.Enums;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.BackgroundJobs;

public class CanceledExpiredOrdersJob(
    EcommerceDbContext ecommerceDb,
    ILogger<CanceledExpiredOrdersJob> logger
)
{
    private readonly EcommerceDbContext _ecommerceDb = ecommerceDb;
    private readonly ILogger<CanceledExpiredOrdersJob> _logger = logger;

    public async Task ExecuteAsync()
    {
        _logger.LogInformation("Starting batch cancellation of expired orders");

        DateTime expirationTime = DateTime.UtcNow.AddDays(-3);

        List<Order>? expiredOrders = await _ecommerceDb
            .Orders.Where(o =>
                o.Status == OrderStatus.PaymentPending && o.UpdatedAt < expirationTime
            )
            .ToListAsync();

        if (expiredOrders.Count == 0)
        {
            _logger.LogInformation("No expired orders found");
            return;
        }

        _logger.LogInformation("Found {count} expired orders", expiredOrders.Count);

        foreach (Order order in expiredOrders)
        {
            order.MarkAsPaymentExpired();
        }

        await _ecommerceDb.SaveChangesAsync();

        _logger.LogInformation("Expired orders have been updated");
    }
}
