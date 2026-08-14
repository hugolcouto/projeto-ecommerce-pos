namespace Ecommerce.Infrastructure.Payment;

public class PaymentSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string ApiBaseUrl { get; set; } = string.Empty;
    public string SuccessUrl { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
}
