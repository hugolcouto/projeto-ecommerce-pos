namespace Ecommerce.Application.Common.ShoppingCart;

public class ProductItemShoppingCartModel(Guid idProduct, int quantity)
{
    public Guid IdProduct { get; set; } = idProduct;
    public int Quantity { get; set; } = quantity;
}
