using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Ecommerce.Infrastructure.Payment;

public class StripePaymentService : IPaymentService
{
    private readonly IOptions<PaymentSettings> _paymentSettings;

    public StripePaymentService(IOptions<PaymentSettings> paymentSettings)
    {
        _paymentSettings = paymentSettings;

        _httpClient = new HttpClient();
        string? authToken = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($"{_paymentSettings.Value.ApiKey}:")
        );
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic",
            authToken
        );
    }

    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public async Task<string> CreateCustomerAsync(PaymentCustomerModel customer)
    {
        var formData = new Dictionary<string, string>
        {
            { "name", customer.FullName },
            { "email", customer.Email },
        };

        if (!string.IsNullOrEmpty(customer.PhoneNumber))
        {
            formData.Add("phone", customer.PhoneNumber);
        }

        var content = new FormUrlEncodedContent(formData);
        HttpResponseMessage? response = await _httpClient.PostAsync(
            $"{_paymentSettings.Value.ApiBaseUrl}/customers",
            content
        );

        response.EnsureSuccessStatusCode();

        string? responseContent = await response.Content.ReadAsStringAsync();
        PaymentCustomerResponseModel? responseModel =
            JsonSerializer.Deserialize<PaymentCustomerResponseModel>(
                responseContent,
                _jsonSerializerOptions
            );

        return responseModel?.Id ?? throw new Exception("Failed to create customer");
    }

    public async Task<PaymentOrderResponseModel> CreateOrderAsync(PaymentOrderModel paymentOrder)
    {
        var formData = new Dictionary<string, string>
        {
            { "success_url", _paymentSettings.Value.SuccessUrl },
            { "mode", "payment" },
            { "customer", paymentOrder.IdExternalCustomer },
            { "branding_settings[display_name]", _paymentSettings.Value.DisplayName },
        };

        for (int i = 0; i < paymentOrder.Items.Count; i++)
        {
            PaymentOrderItemModel? item = paymentOrder.Items[i];
            long unitAmountDecimal = (long)(item.Price * 100); // Convert to cents

            formData.Add($"line_items[{i}][quantity]", item.Quantity.ToString());
            formData.Add($"line_items[{i}][price_data][currency]", _paymentSettings.Value.Currency);
            formData.Add($"line_items[{i}][price_data][product_data][name]", item.Name);
            formData.Add(
                $"line_items[{i}][price_data][unit_amount_decimal]",
                unitAmountDecimal.ToString()
            );
        }

        var content = new FormUrlEncodedContent(formData);
        HttpResponseMessage? response = await _httpClient.PostAsync(
            $"{_paymentSettings.Value.ApiBaseUrl}/checkout/sessions",
            content
        );

        response.EnsureSuccessStatusCode();

        string? responseContent = await response.Content.ReadAsStringAsync();
        PaymentOrderResponseModel? responseModel =
            JsonSerializer.Deserialize<PaymentOrderResponseModel>(
                responseContent,
                _jsonSerializerOptions
            );

        return responseModel ?? throw new Exception("Failed to create checkout session");
    }
}
