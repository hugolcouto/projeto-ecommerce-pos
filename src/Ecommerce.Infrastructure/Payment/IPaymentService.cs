namespace Ecommerce.Infrastructure.Payment;

public interface IPaymentService
{
    Task<string> CreateCustomerAsync(PaymentCustomerModel customer); // Renamed from CreateCustomerAsync to match implementation
    Task<PaymentOrderResponseModel> CreateOrderAsync(PaymentOrderModel paymentOrder); // Renamed from CreateOrderAsync to match implementation
}
