namespace Ecommerce.Application.Queries.Products.GetProductDetails;

public class GetProductDetailsQuery(Guid idProduct)
{
    public Guid IdProduct { get; set; } = idProduct;
}