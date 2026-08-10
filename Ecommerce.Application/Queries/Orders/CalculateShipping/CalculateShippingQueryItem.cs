namespace Ecommerce.Application;

public class CalculateShippingQueryItem
{
    public CalculateShippingQueryItem(Guid idProduct, int quantity)
    {
        IdProduct = idProduct;
        Quantity = quantity;
    }

    public Guid IdProduct { get; set; }
    public int Quantity { get; set; }
}
