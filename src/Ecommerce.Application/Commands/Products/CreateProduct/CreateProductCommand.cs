namespace Ecommerce.Application.Commands.ProductsCommands.Products.CreateProduct;

public class CreateProductCommand
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Brand { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Guid IdCategory { get; set; }
}
