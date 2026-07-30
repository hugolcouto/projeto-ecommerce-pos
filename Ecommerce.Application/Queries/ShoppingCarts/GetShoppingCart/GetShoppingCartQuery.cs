namespace Ecommerce.Application.Queries.ShoppingCarts;

public class GetShoppingCartQuery
{
    public GetShoppingCartQuery(Guid idCustomer)
    {
        IdCustomer = idCustomer;
    }

    public Guid IdCustomer { get; set; }
}
