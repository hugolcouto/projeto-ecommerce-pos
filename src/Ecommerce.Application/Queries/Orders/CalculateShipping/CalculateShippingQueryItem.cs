namespace Ecommerce.Application;

public class CalculateShippingQueryItem(Guid idProduct, int quantity)
{
    public Guid IdProduct { get; set; } = idProduct;
    public int Quantity { get; set; } = quantity;
}
