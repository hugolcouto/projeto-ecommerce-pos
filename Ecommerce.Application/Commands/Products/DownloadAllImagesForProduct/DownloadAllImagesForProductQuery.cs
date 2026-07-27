using System;

namespace Ecommerce.Application.Commands.Products.DownloadAllImagesForProduct;

public class DownloadAllImagesForProductQuery
{
    public DownloadAllImagesForProductQuery(Guid idProduct)
    {
        IdProduct = idProduct;
    }

    public Guid IdProduct { get; set; }
}
