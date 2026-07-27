namespace Ecommerce.Application.Queries.Products.GetProductDetails;

public class GetProductDetailsQuery
{
    public GetProductDetailsQuery(Guid idProduct)
    {
        IdProduct = idProduct;
    }

    public Guid IdProduct { get; set; }
}