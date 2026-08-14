using System;

namespace Ecommerce.Application.Commands.Products.DownloadImageForProduct;

public class DownloadImageForProductQuery(Guid idProductImage)
{
    public Guid IdProductImage { get; set; } = idProductImage;
}
