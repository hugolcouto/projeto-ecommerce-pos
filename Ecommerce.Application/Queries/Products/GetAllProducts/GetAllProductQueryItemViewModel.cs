namespace Ecommerce.Application.Queries.Products.GetAllProducts;

public class GetAllProductQueryItemViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
}