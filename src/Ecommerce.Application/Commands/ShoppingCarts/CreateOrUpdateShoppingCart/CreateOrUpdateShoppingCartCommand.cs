using Ecommerce.Application.Common.ShoppingCart;

namespace Ecommerce.Application.Commands.ShoppingCarts.CreateOrUpdateShoppingCart;

public class CreateOrUpdateShoppingCartCommand(
    Guid idCustomer,
    List<ProductItemShoppingCartModel> items
    )
{
    public Guid IdCustomer { get; set; } = idCustomer;
    public List<ProductItemShoppingCartModel> Items { get; set; } = items;
}
