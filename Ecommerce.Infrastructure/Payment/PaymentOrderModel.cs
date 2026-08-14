namespace Ecommerce.Infrastructure.Payment;

public class PaymentOrderModel
{
    public List<PaymentOrderItemModel> Items { get; set; }
    public string IdExternalCustomer { get; set; }
}
