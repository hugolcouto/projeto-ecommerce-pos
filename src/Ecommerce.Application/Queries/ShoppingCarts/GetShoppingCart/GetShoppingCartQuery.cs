namespace Ecommerce.Application.Queries.ShoppingCarts;

public class GetShoppingCartQuery(Guid idCustomer)
{
    public Guid IdCustomer { get; set; } = idCustomer;
}
