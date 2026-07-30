using Ecommerce.Application.Common.ShoppingCart;

namespace Ecommerce.Application.Commands.ShoppingCarts.CreateOrUpdateShoppingCart;

public class CreateOrUpdateShoppingCartCommand
{
    public CreateOrUpdateShoppingCartCommand(
        Guid idCustomer,
        List<ProductItemShoppingCartModel> items
    )
    {
        IdCustomer = idCustomer;
        Items = items;
    }

    public Guid IdCustomer { get; set; }
    public List<ProductItemShoppingCartModel> Items { get; set; }
}
