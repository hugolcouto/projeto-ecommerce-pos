using System;

namespace Ecommerce.Application.Commands.Products.DownloadAllImagesForProduct;

public class DownloadAllImagesForProductQuery(Guid idProduct)
{
    public Guid IdProduct { get; set; } = idProduct;
}
